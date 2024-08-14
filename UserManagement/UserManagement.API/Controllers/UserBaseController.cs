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
            if (!response.IsSuccessful)
            {
                return StatusCode((int)httpStatusCode, response);
            }

            return StatusCode((int)httpStatusCode, response);
        }



    }
}
