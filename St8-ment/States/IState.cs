using System.Threading.Tasks;

namespace St8Ment.States
{
    public interface IState
    {
        Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction;
    }

    public interface IState<out TSubject> : IState, IStateHandle<TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        StateId Id { get; }
    }
}
