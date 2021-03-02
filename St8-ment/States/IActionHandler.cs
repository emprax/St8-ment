using System.Threading.Tasks;

namespace St8Ment.States
{
    public interface IActionHandler<TAction, TSubject>
        where TSubject : class, IStateSubject<TSubject>
        where TAction : class, IAction
    {
        Task<StateId> Execute(TAction action, IStateView<TSubject> state);
    }
}
