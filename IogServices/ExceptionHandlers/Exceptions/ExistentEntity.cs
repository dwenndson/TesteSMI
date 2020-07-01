using System;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.ExceptionHandlers.Exceptions
{
    internal class ExistentEntityProblemDetails : ProblemDetails {}

    internal class ExistentEntityException : Exception
    {
        public string Title = "Entidade já existente";
        public string Detail { get; set; }

        public ExistentEntityException(string detail)
        {
            Detail = detail;
        }
    }
}