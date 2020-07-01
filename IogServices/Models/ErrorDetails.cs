using System;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2.HPack;
using Newtonsoft.Json;

namespace IogServices.Models
{
    public class ErrorDetails
    {
        public string Message { get; set; }


        public ErrorDetails(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}