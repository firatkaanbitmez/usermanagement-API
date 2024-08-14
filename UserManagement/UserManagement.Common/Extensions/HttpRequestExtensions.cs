using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UserManagement.Common.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsApiRequest(this HttpRequest request)
        {
            return request.Path.StartsWithSegments(new PathString("/api"));
        }
    }

}
