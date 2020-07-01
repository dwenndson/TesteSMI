using System;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.ExceptionHandlers.Exceptions
{
    internal class BadRequestProblemDetails : ProblemDetails {}
    
    internal class BadRequestException : Exception
    {
        public string Title = "Dados Inválidos";
        public string Detail { get; set; }

        public BadRequestException(string detail)
        {
            Detail = detail;
        }
    }
}