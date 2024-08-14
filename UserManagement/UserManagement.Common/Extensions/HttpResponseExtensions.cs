using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace UserManagement.Common.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task WriteJsonAsync<T>(this HttpResponse response, T obj, string contentType = "application/json", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            response.ContentType = contentType;
            response.StatusCode = (int)statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(obj));
        }
    }

}
