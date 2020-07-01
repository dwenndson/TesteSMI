using System;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.ExceptionHandlers.Exceptions
{
    internal class InvalidConstraintProblemDetails : ProblemDetails {}

    internal class InvalidConstraintException : Exception
    {
        public string Title = "Chave inválida";
        public string Detail { get; set; }
        
        public InvalidConstraintException(string detail)
        {
            Detail = detail;
        }
    }
}