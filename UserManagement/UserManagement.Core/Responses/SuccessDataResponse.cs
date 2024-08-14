using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Responses
{
    public class SuccessDataResponse<T> : UserDataResponse<T>
    {
        public new T Data { get; set; }

        public SuccessDataResponse(T data, bool isSuccessful) : base(data, isSuccessful)
        {
            Data = data;
        }
    }

    public class SuccessDataResponse
    {
        public bool IsSuccessful { get; set; } = true;
    }

}
