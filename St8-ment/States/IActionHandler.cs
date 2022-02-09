using System.Threading.Tasks;

namespace St8Ment.States
{
    public interface IActionHandler { }

    public interface IActionHandler<in TAction, in TSubject> : IActionHandler
        where TSubject : StateSubject
        where TAction : class, IAction
    {
        Task Execute(TAction action, IStateHandle<TSubject> state);
    }
}
