using System.Threading;
using System.Threading.Tasks;

namespace St8Ment.States.Abstractions.Core;

public interface IActionHandler<TSubject> where TSubject : class, IStateSubject<TSubject>;

public interface IActionHandler<TSubject, TAction> : IActionHandler<TSubject>
    where TSubject : class, IStateSubject<TSubject>
    where TAction : IAction<TSubject>
{
    Task ExecuteAsync(TAction action, IStateHandle<TSubject> handle, CancellationToken cancellationToken);
}