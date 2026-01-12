using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("errors/{code}")]
[ApiController]
public class ErrorsController : ControllerBase
{
    public IActionResult Error(int code)
    {
        return new ObjectResult(new ApiErrorResponse(code));
    }
}
