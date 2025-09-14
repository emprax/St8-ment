using System.Threading;
using System.Threading.Tasks;

namespace St8Ment.States.Abstractions.Core;

public interface IStateReducer<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    Task<StateRepsonse> ExecuteAsync<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction<TSubject>;
}