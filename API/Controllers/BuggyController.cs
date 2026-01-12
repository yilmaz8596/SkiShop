using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseAPIController
    {

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorizedRequest()
        {
            throw new Exception("This is a test exception for unauthorized request");
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            throw new Exception("This is a test exception for not found request");
        }
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            throw new Exception("This is a test exception for bad request");
        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a test exception");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }
    }
}
