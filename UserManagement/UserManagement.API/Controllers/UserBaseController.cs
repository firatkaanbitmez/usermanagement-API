using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserManagement.Core.Responses;

namespace UserManagement.API.Controllers
{
    public class UserBaseController : ControllerBase
    {
        protected IActionResult ApiResponse(HttpStatusCode httpStatusCode)
        {
            return StatusCode((int)httpStatusCode);
        }

        protected IActionResult ApiResponse<T>(UserDataResponse<T> response)
        {
            if (response == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            else if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }
        }


        protected IActionResult ApiResponse<T>(HttpStatusCode httpStatusCode, UserDataResponse<T> response)
        {
            return StatusCode((int)httpStatusCode, response);
        }



        private IActionResult SendResponse(dynamic result)
        {
            if (result.IsSuccessful)
            {
                switch (result.HttpStatusCode)
                {
                    case (int)HttpStatusCode.OK:
                        return Ok(result);
                    case (int)HttpStatusCode.Created:
                        return Created("", result);
                    case (int)HttpStatusCode.Accepted:
                        return Accepted(result);
                    case (int)HttpStatusCode.NoContent:
                        return NoContent();
                }
            }
            else
            {
                switch (result.HttpStatusCode)
                {
                    case (int)HttpStatusCode.BadRequest:
                        return BadRequest(new ErrorDataResponse(result.ErrorMessageList, HttpStatusCode.BadRequest));
                    case (int)HttpStatusCode.Unauthorized:
                        return Unauthorized(new { Message = "Unauthorized request" });
                    case (int)HttpStatusCode.Forbidden:
                        return Forbid();
                    case (int)HttpStatusCode.NotFound:
                        return NotFound();
                    default:
                        return BadRequest();
                }
            }

            return BadRequest();
        }
    }
}
