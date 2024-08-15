using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Responses
{
    public class UserDataResponse<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; }
        public List<string> Errors { get; set; }
        public string? ErrorMessage { get; set; }

        public UserDataResponse(T? data, bool isSuccessful, List<string>? errors = null, string? errorMessage = null)
        {
            Data = data;
            IsSuccessful = isSuccessful;
            Errors = errors ?? new List<string>();
            ErrorMessage = errorMessage;
        }

        public bool HasErrorMessage() => !string.IsNullOrEmpty(ErrorMessage);

        public void AddErrorMessageToList(List<string> errors)
        {
            if (HasErrorMessage() && ErrorMessage != null)  // Null kontrolü eklenmiş hali
            {
                errors.Add(ErrorMessage);
            }
        }

        // Fail metodu tanımlama
        public static UserDataResponse<T> Fail(string errorMessage, List<string>? errors = null)
        {
            return new UserDataResponse<T>(default, false, errors, errorMessage);
        }
    }

}
