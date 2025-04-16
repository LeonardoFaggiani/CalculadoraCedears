using System.Net;

using CalculadoraCedears.Api.Infrastructure.Exceptions;
using CalculadoraCedears.Api.Infrastructure.Exceptions.Builder;
using CalculadoraCedears.Api.Infrastructure.Filters;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using Moq;

namespace CalculadoraCedears.Api.Unit.Tests.Infrastructure.Filters
{
    public class CustomExceptionFilterTests
    {
        private IServiceProvider ServiceProvider;
        private IExceptionMessageBuilder ExceptionMessageBuilder;

        public CustomExceptionFilterTests()
        {
            ServiceProvider = Mock.Of<IServiceProvider>();
            ExceptionMessageBuilder = Mock.Of<IExceptionMessageBuilder>();

            Mock.Get(ExceptionMessageBuilder).Setup(x => x.WithContext(It.IsAny<ExceptionContext>(), It.IsAny<HttpStatusCode>())).Returns(ExceptionMessageBuilder);
            Mock.Get(ExceptionMessageBuilder).Setup(x => x.WithExceptionMessage()).Returns(ExceptionMessageBuilder);
            Mock.Get(ExceptionMessageBuilder).Setup(x => x.WithStackTrace()).Returns(ExceptionMessageBuilder);
            Mock.Get(ExceptionMessageBuilder).Setup(x => x.WithSupportId()).Returns(ExceptionMessageBuilder);
            Mock.Get(ExceptionMessageBuilder).Setup(x => x.Build()).Returns(new ExceptionMessage("Error", "trace", "id"));
        }

        public class The_Constructor : CustomExceptionFilterTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_exceptionMessageBuilder_is_null()
            {
                // act & assert
                Assert.Throws<ArgumentNullException>(() => new CustomExceptionFilter(null));
            }
        }

        public class The_Method_OnException : CustomExceptionFilterTests
        {

            [Fact]
            public void OnExceptionTests_badRequest_shouldWork()
            {
                //Arrange
                var context = GetActionContext();

                context.HttpContext.RequestServices = ServiceProvider;

                var exContext = new ExceptionContext(context, new List<IFilterMetadata>())
                {
                    Exception = new Exception("Error")
                };

                var result = new CustomExceptionFilter(ExceptionMessageBuilder);

                //Act
                result.OnException(exContext);

                //Assert
                Assert.Equal((int)HttpStatusCode.InternalServerError, exContext.HttpContext.Response.StatusCode);
            }

            private static ActionContext GetActionContext()
                   => new ActionContext
                   {
                       HttpContext = new DefaultHttpContext(),
                       RouteData = new RouteData(),
                       ActionDescriptor = new ActionDescriptor(),
                   };

        }
    }
}
