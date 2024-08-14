using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Responses
{
    public class ErrorDataResponse
    {
        public List<string> Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ErrorDataResponse(List<string> errors, HttpStatusCode statusCode)
        {
            Errors = errors;
            StatusCode = statusCode;
        }
    }

}
