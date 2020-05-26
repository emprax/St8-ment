using System.Threading;
using System.Threading.Tasks;

namespace St8_ment
{
    public interface IState { };

    public interface IState<TContext> : IState where TContext : IStateContext<TContext>
    {
        TContext Context { get; }

        Task<bool> Accept<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction;
    }
}
