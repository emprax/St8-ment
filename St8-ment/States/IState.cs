using System.Threading.Tasks;

namespace St8_ment.States
{
    public interface IState<TContext> : IStateView<TContext> where TContext : class, IStateContext<TContext>
    {
        Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction;
    }
}
