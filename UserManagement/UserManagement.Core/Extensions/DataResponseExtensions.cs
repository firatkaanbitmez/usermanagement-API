using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Responses;

namespace UserManagement.Core.Extensions
{
    public static class DataResponseExtensions
    {
        public static UserDataResponse<T> ToDataResponse<T>(this T data, bool isSuccessful = true, List<string>? errors = null)
        {
            return new UserDataResponse<T>(data, isSuccessful, errors);
        }

        public static UserDataResponse<T> Fail<T>(this string errorMessage, List<string>? errors = null)
        {
            return new UserDataResponse<T>(default, false, errors, errorMessage);
        }
    }
}
