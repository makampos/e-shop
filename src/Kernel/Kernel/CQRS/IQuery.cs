using MediatR;

namespace Kernel.CQRS;

public interface IQuery<out T> : IRequest<T> where T : notnull
{

}