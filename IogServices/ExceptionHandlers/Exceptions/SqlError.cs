using Microsoft.AspNetCore.Mvc;

namespace IogServices.ExceptionHandlers.Exceptions
{
    public class SqlError
    {
        internal class SqlErrorProblemDetails: ProblemDetails {}
    }
}