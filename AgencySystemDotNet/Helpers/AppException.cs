using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PressAgencyApp.Helpers
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public AppException(HttpStatusCode code,string message):base(message)
        {
            StatusCode = code;
        }

        public AppException() : base()
        {
        }

        public AppException(string message) : base(message)
        {
        }

        public AppException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public AppException(HttpStatusCode code,string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = code;
        }
    }
}
