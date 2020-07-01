using System;
using System.Data;
using System.Data.SqlClient;
using Hellang.Middleware.ProblemDetails;
using IogServices.ExceptionHandlers.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using InvalidConstraintException = IogServices.ExceptionHandlers.Exceptions.InvalidConstraintException;

namespace IogServices.ExceptionHandlers
{
    public static class ProblemDetailsExtensions
    {
        public static void CustomAddProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = _ => true;
                options.Map<InvalidConstraintException>(exception => new InvalidConstraintProblemDetails
                {
                    Title = exception.Title,
                    Detail = exception.Detail,
                    Type = exception.GetType().Name,
                    Status = StatusCodes.Status400BadRequest
                });
                options.Map<ExistentEntityException>(exception => new ExistentEntityProblemDetails
                {
                    Title = exception.Title,
                    Detail = exception.Detail,
                    Type = exception.GetType().Name,
                    Status = StatusCodes.Status400BadRequest
                });
                options.Map<NotNullForeignKeyReferenceException>(exception =>
                    new NotNullForeignKeyReferenceProblemDetails()
                    {
                        Title = exception.Title,
                        Detail = exception.Detail,
                        Type = exception.GetType().Name,
                        Status = StatusCodes.Status400BadRequest
                    });
                options.Map<BadRequestException>(exception =>
                    new BadRequestProblemDetails()
                    {
                        Title = exception.Title,
                        Detail = exception.Detail,
                        Type = exception.GetType().Name,
                        Status = StatusCodes.Status400BadRequest
                    });
                options.Map<BadCredentialsException>(exception =>
                    new BadCredentialsProblemDetails()
                    {
                        Title = exception.Title,
                        Detail = exception.Detail,
                        Type = exception.GetType().Name,
                        Status = StatusCodes.Status400BadRequest
                    });
            });
        }

        public static IServiceCollection ConfigureProblemDetailsModelState(this IServiceCollection services)
        {
            return services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details"
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = {"application/problem+json", "application/problem+xml"}
                    };
                };
            });
        }
    }
}