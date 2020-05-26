using System.Threading;
using System.Threading.Tasks;

namespace St8_ment
{
    public interface IStateContext<TContext> where TContext : IStateContext<TContext>
    {
        void SetState<TState>(TState state) where TState : class, IState<TContext>;

        Task<bool> Apply<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction;
    }
}
