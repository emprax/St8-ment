using System.Threading.Tasks;

namespace St8Ment.States
{
    public interface IState<TSubject> : IStateView<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction;
    }
}
