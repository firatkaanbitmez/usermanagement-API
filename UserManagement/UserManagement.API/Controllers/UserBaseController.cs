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
            else if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                response.AddErrorMessageToList(errors);
                return StatusCode((int)HttpStatusCode.BadRequest, new UserDataResponse<T>(default, false, errors));
            }
            else
            {
                return Ok(response);
            }
        }

        protected IActionResult ApiResponse<T>(HttpStatusCode httpStatusCode, UserDataResponse<T> response)
        {
            var errors = new List<string>();
            response.AddErrorMessageToList(errors);
            return StatusCode((int)httpStatusCode, new UserDataResponse<T>(response.Data, response.IsSuccessful, errors));
        }



    }
}
