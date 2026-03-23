using ciit_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ciit_api.Controllers
{
    [Route("api/enquiries")]
    [ApiController]
    public class EnquiriesController : BaseApiController
    {
        private readonly CiitstudContext _context;
        private readonly ILogger<EnquiriesController> _logger;

        public EnquiriesController(CiitstudContext context, ILogger<EnquiriesController> logger)
        {
            _context = context;
            _logger = logger;
        }




    }
}
