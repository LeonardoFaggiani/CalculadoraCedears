﻿using MediatR;

using Moq;

namespace CalculadoraCedears.Api.Unit.Tests.Base
{
    public abstract class BaseTestClass<T>
        where T : class
    {
        public T Sut { get; set; }
        public IMediator Mediator { get; set; }
        protected CancellationToken CancellationToken => CancellationToken.None;

        public BaseTestClass()
        {
            Mediator = Mock.Of<IMediator>();
        }
    }
}
