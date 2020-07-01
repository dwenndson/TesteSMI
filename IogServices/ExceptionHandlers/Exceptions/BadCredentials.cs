using System;
using Microsoft.AspNetCore.Mvc;

namespace IogServices.ExceptionHandlers.Exceptions
{
    internal class BadCredentialsProblemDetails : ProblemDetails
    {
    }

    internal class BadCredentialsException : Exception
    {
        public string Title = "Falha ao tentar fazer login";
        public string Detail = "E-mail ou senha incorretos";
    }
}