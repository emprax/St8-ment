using System.Threading.Tasks;

namespace St8Ment.States
{
    public interface IStateSubject<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        void SetState(IState<TSubject> state);

        Task<StateResponse> Apply<TAction>(TAction action) where TAction : class, IAction;
    }
}
