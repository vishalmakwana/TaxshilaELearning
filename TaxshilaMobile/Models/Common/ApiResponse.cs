using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Version { get; set; }
        public string StackTrace { get; set; }
        public string RequestDateTime { get; set; }
        public T ResponseContent { get; set; }
     
        public double TotalResponceTime { get; set; }
        public DateTime RequestDatetime { get; set; }
        public void Fail(string error)
        {
            Success = false;
            Message = error;
        }
        public Response()
        {
                
        }

    }
}
