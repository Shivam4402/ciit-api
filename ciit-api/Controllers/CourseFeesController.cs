using ciit_api.DTOs.CourseFee;
using ciit_api.Models;
using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.IO.Image;

namespace ciit_api.Controllers
{
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

                // 🔹 LOGO
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mainlogo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    var image = new iText.Layout.Element.Image(
                        iText.IO.Image.ImageDataFactory.Create(logoPath))
                        .ScaleToFit(100, 100);
                    document.Add(image);
                }

                // 🔹 COMPANY NAME
                document.Add(new Paragraph("CIIT Institute").SimulateBold().SetFontSize(18));

                document.Add(new Paragraph("Pune, India | +91-XXXXXXXXXX"));

                document.Add(new Paragraph("\n"));

                // 🔹 INVOICE HEADER
                document.Add(new Paragraph("FEE INVOICE")
                    .SimulateBold().SetFontSize(16));

                document.Add(new Paragraph($"Date: {DateTime.Now:dd MMM yyyy}"));
                document.Add(new Paragraph($"Student: {dto.StudentName}"));

                document.Add(new Paragraph("\n"));

                // 🔹 COURSE INFO
                document.Add(new Paragraph($"Course: {dto.CourseName}"));
                document.Add(new Paragraph($"Registration Date: {dto.RegistrationDate}"));
                document.Add(new Paragraph($"Status: {dto.Status}"));

                document.Add(new Paragraph("\n"));

                // 🔹 FEE TABLE
                var feeTable = new Table(5);
                feeTable.AddHeaderCell("Total");
                feeTable.AddHeaderCell("Discount");
                feeTable.AddHeaderCell("Payable");
                feeTable.AddHeaderCell("Paid");
                feeTable.AddHeaderCell("Due");

                feeTable.AddCell(dto.TotalFee.ToString("C"));
                feeTable.AddCell(dto.Discount.ToString("C"));
                feeTable.AddCell(dto.PayableFee.ToString("C"));
                feeTable.AddCell(dto.PaidFee.ToString("C"));
                feeTable.AddCell(dto.DueFee.ToString("C"));

                document.Add(feeTable);

                document.Add(new Paragraph("\n"));

                // 🔹 PAYMENT HISTORY TABLE
                document.Add(new Paragraph("Payment History").SimulateBold());

                var paymentTable = new Table(3);
                paymentTable.AddHeaderCell("Date");
                paymentTable.AddHeaderCell("Mode");
                paymentTable.AddHeaderCell("Amount");

                foreach (var p in dto.Payments)
                {
                    paymentTable.AddCell(p.PaymentDate);
                    paymentTable.AddCell(p.PaymentMode);
                    paymentTable.AddCell(p.Amount.ToString("C"));
                }

                document.Add(paymentTable);

                document.Add(new Paragraph("\n"));

                // 🔹 SUMMARY
                document.Add(new Paragraph($"Total Paid: {dto.PaidFee:C}").SimulateBold());
                document.Add(new Paragraph($"Outstanding Due: {dto.DueFee:C}")
                    .SimulateBold()
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.RED));

                document.Close();

                var pdfBytes = stream.ToArray();
                var base64 = Convert.ToBase64String(pdfBytes);  

                return Ok(base64);
            }
        }
    }

}
