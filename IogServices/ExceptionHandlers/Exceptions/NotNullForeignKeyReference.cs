using System;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.ExceptionHandlers.Exceptions
{
    internal class NotNullForeignKeyReferenceProblemDetails : ProblemDetails {}

    internal class NotNullForeignKeyReferenceException : Exception
    {
        public string Title = "Referência não pode ser nula";
        public string Detail { get; set; }

        public NotNullForeignKeyReferenceException(string detail)
        {
            Detail = detail;
        }
    }
}