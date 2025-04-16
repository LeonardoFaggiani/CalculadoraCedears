using MediatR;

using NetDevPack.Data;

namespace CalculadoraCedears.Api.Application.Base
{
    public abstract class CommandHandler<T> : AsyncRequestHandler<T> where T : IRequest
    {
        protected async Task Commit(IUnitOfWork uow)
        {
            await uow.Commit();
        }
    }
}
