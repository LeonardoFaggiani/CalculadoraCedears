﻿using CalculadoraCedears.Api.Infrastructure.Exceptions;
using CalculadoraCedears.Api.Infrastructure.Exceptions.Builder;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CalculadoraCedears.Api.Infrastructure.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly Dictionary<Type, HttpStatusCode> exceptionHttpStatusCodes = new()
        {
            { typeof(ValidationException), HttpStatusCode.BadRequest },
            { typeof(AlreadyExistsCedearException), HttpStatusCode.Conflict },
        };

        private readonly IExceptionMessageBuilder exceptionMessageBuilder;

        public CustomExceptionFilter(IExceptionMessageBuilder exceptionMessageBuilder)
        {
            this.exceptionMessageBuilder = exceptionMessageBuilder ?? throw new ArgumentNullException(nameof(exceptionMessageBuilder));
        }

        public void OnException(ExceptionContext context)
        {
            var code = HttpStatusCode.InternalServerError;

            var exceptionMessage = exceptionMessageBuilder
                               .WithContext(context, code)
                               .WithExceptionMessage()
                               .WithStackTrace()
                               .WithSupportId()
                               .Build();

            IActionResult result = new JsonResult(exceptionMessage);

            if (exceptionHttpStatusCodes.ContainsKey(context.Exception.GetType()))
            {
                code = exceptionHttpStatusCodes[context.Exception.GetType()];

                if (context.Exception.Data.Contains("changeState"))
                {
                    result = new JsonResult(context.Exception.Data["changeState"]);
                }

            }
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = result;
        }
    }
}
