using ciit_api.DTOs.CourseFee;
using ciit_api.Models;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Authorize]
    [Route("api/course-fees")]
    [ApiController]
    public class CourseFeesController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<CourseFeesController> _logger;

        public CourseFeesController(CiitstudContext context, ILogger<CourseFeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFees()
        {
            try
            {
                var data = await _context.TbltrainingCourseFees
                    .Where(x => x.Flag == 0)
                    .OrderBy(x => x.FeeId)
                    .Select(x => new CourseFeeResponseDto
                    {
                        FeeId = x.FeeId,
                        CourseId = x.CourseId,
                        FeesAmount = x.FeesAmount,
                        Gst = x.Gst,
                        FeeMode = x.FeeMode,
                        FeesChangeDate = x.FeesChangeDate
                    })
                    .ToListAsync();

                return ApiResponse(true, "Course fees fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllFees");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeeById(int id)
        {
            try
            {
                var data = await _context.TbltrainingCourseFees
                    .Where(x => x.FeeId == id && x.Flag == 0)
                    .Select(x => new CourseFeeResponseDto
                    {
                        FeeId = x.FeeId,
                        CourseId = x.CourseId,
                        FeesAmount = x.FeesAmount,
                        Gst = x.Gst,
                        FeeMode = x.FeeMode,
                        FeesChangeDate = x.FeesChangeDate
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                    return ApiResponse(false, $"Fee with ID {id} not found", statusCode: 404);

                return ApiResponse(true, "Course fee fetched successfully", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetFeeById {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFee([FromBody] CreateCourseFeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(false, "Validation failed",
                    errors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(),
                    statusCode: 400);
            }

            try
            {
                var courseExists = await _context.TbltrainingCourses
                    .AnyAsync(c => c.CourseId == dto.CourseId && c.Flag == 0);

                if (!courseExists)
                    return ApiResponse(false, $"Invalid CourseId {dto.CourseId}", statusCode: 400);

                var entity = new TbltrainingCourseFee
                {
                    CourseId = dto.CourseId,
                    FeesAmount = dto.FeesAmount,
                    Gst = dto.Gst,
                    FeeMode = dto.FeeMode,
                    FeesChangeDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Flag = 0
                };

                _context.TbltrainingCourseFees.Add(entity);
                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course fee created successfully", new
                {
                    entity.FeeId,
                    entity.CourseId,
                    entity.FeesAmount
                }, statusCode: 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateFee");
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFee(int id, [FromBody] UpdateCourseFeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResponse(false, "Validation failed",
                    errors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(),
                    statusCode: 400);
            }

            try
            {
                var entity = await _context.TbltrainingCourseFees
                    .Where(x => x.FeeId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Fee with ID {id} not found", statusCode: 404);

                entity.FeesAmount = dto.FeesAmount;
                entity.Gst = dto.Gst;
                entity.FeeMode = dto.FeeMode;
                entity.FeesChangeDate = DateOnly.FromDateTime(DateTime.UtcNow);


                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course fee updated successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateFee {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFee(int id)
        {
            try
            {
                var entity = await _context.TbltrainingCourseFees
                    .Where(x => x.FeeId == id && x.Flag == 0)
                    .FirstOrDefaultAsync();

                if (entity == null)
                    return ApiResponse(false, $"Fee with ID {id} not found", statusCode: 404);

                entity.Flag = 1;

                await _context.SaveChangesAsync();

                return ApiResponse(true, "Course fee deleted successfully", statusCode: 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteFee {Id}", id);
                return ApiResponse(false, "Something went wrong", error: ex.Message, statusCode: 500);
            }
        }

        [HttpPost("generate-fee-invoice")]
        public IActionResult GenerateFeeInvoice([FromBody] CourseFeePdfDto dto)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var writer = new iText.Kernel.Pdf.PdfWriter(stream);
                var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                var document = new iText.Layout.Document(pdf);

                // 🔹 Fonts
                var boldFont = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                var normalFont = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);

                // 🔹 Colors
                var primaryColor = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);
                var lightGray = new iText.Kernel.Colors.DeviceRgb(240, 240, 240);
                var darkGray = new iText.Kernel.Colors.DeviceRgb(45, 45, 45);

                // ================= HEADER =================
                var headerTable = new Table(2).UseAllAvailableWidth();

                // Logo
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mainlogo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    var logo = new iText.Layout.Element.Image(
                        iText.IO.Image.ImageDataFactory.Create(logoPath))
                        .ScaleToFit(100, 100);

                    headerTable.AddCell(new Cell().Add(logo).SetBorder(Border.NO_BORDER));
                }
                else
                {
                    headerTable.AddCell(new Cell().SetBorder(Border.NO_BORDER));
                }

                // Company Details
                var company = new Paragraph()
                    .Add(new Text("CIIT Institute\n").SetFont(boldFont).SetFontSize(16))
                    .Add("Pune, India\n+91-7028565830")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);

                headerTable.AddCell(new Cell().Add(company).SetBorder(Border.NO_BORDER));

                document.Add(headerTable);

                document.Add(new Paragraph("\n"));

                // ================= INVOICE TITLE =================
                var title = new Paragraph("FEE INVOICE")
                    .SetFont(boldFont)
                    .SetFontSize(18)
                    .SetFontColor(darkGray);

                document.Add(title);

                // Invoice Meta
                var metaTable = new Table(2).UseAllAvailableWidth();

                metaTable.AddCell(new Cell().Add(new Paragraph($"Invoice Date: {DateTime.Now:dd MMM yyyy}"))
                    .SetBorder(Border.NO_BORDER));

                metaTable.AddCell(new Cell().Add(new Paragraph($"Invoice No: INV-{DateTime.Now.Ticks}"))
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER));

                document.Add(metaTable);

                document.Add(new Paragraph("\n"));

                // ================= STUDENT INFO =================
                var infoTable = new Table(2).UseAllAvailableWidth();

                infoTable.AddCell(new Cell().Add(new Paragraph($"Student: {dto.StudentName}")).SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"Course: {dto.CourseName}")).SetBorder(Border.NO_BORDER));

                infoTable.AddCell(new Cell().Add(new Paragraph($"Registration Date: {dto.RegistrationDate}")).SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"Status: {dto.Status}")).SetBorder(Border.NO_BORDER));

                document.Add(infoTable);

                document.Add(new Paragraph("\n"));

                // ================= FEE TABLE =================
                var feeTable = new Table(5).UseAllAvailableWidth();

                string[] headers = { "Total", "Discount", "Payable", "Paid", "Due" };

                foreach (var h in headers)
                {
                    feeTable.AddHeaderCell(new Cell()
                        .Add(new Paragraph(h).SetFont(boldFont).SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE))
                        .SetBackgroundColor(primaryColor)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                }

                feeTable.AddCell(new Cell().Add(new Paragraph("₹ " + dto.TotalFee.ToString("N0"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                feeTable.AddCell(new Cell().Add(new Paragraph("₹ " + dto.Discount.ToString("N0"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                feeTable.AddCell(new Cell().Add(new Paragraph("₹ " + dto.PayableFee.ToString("N0"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                feeTable.AddCell(new Cell().Add(new Paragraph("₹ " + dto.PaidFee.ToString("N0"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                feeTable.AddCell(new Cell().Add(new Paragraph("₹ " + dto.DueFee.ToString("N0"))).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                document.Add(feeTable);

                document.Add(new Paragraph("\n"));

                // ================= PAYMENT HISTORY =================
                document.Add(new Paragraph("Payment History")
                    .SetFont(boldFont)
                    .SetFontSize(14));

                var paymentTable = new Table(3).UseAllAvailableWidth();

                string[] payHeaders = { "Date", "Mode", "Amount" };

                foreach (var h in payHeaders)
                {
                    paymentTable.AddHeaderCell(new Cell()
                        .Add(new Paragraph(h).SetFont(boldFont))
                        .SetBackgroundColor(lightGray));
                }

                foreach (var p in dto.Payments)
                {
                    paymentTable.AddCell(p.PaymentDate);
                    paymentTable.AddCell(p.PaymentMode);
                    paymentTable.AddCell("₹ " + p.Amount.ToString("N0"));
                }

                document.Add(paymentTable);

                document.Add(new Paragraph("\n"));

                // ================= SUMMARY =================
                var summaryTable = new Table(2).UseAllAvailableWidth();

                summaryTable.AddCell(new Cell()
                    .Add(new Paragraph("Total Paid").SetFont(boldFont))
                    .SetBorder(Border.NO_BORDER));

                summaryTable.AddCell(new Cell()
                    .Add(new Paragraph("₹ " + dto.PaidFee.ToString("N0")).SetFont(boldFont))
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER));

                summaryTable.AddCell(new Cell()
                    .Add(new Paragraph("Outstanding Due").SetFont(boldFont))
                    .SetBorder(Border.NO_BORDER));

                summaryTable.AddCell(new Cell()
                    .Add(new Paragraph("₹ " + dto.DueFee.ToString("N0"))
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.RED)
                    .SetFont(boldFont))
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER));

                document.Add(summaryTable);

                document.Add(new Paragraph("\n"));

                // ================= FOOTER =================
                document.Add(new Paragraph("Thank you for choosing CIIT Institute!")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(10)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.GRAY));

                document.Close();

                var pdfBytes = stream.ToArray();
                var base64 = Convert.ToBase64String(pdfBytes);

                return Ok(base64);
            }
        }
    }

}
    