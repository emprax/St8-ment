namespace St8Ment.States.Abstractions.Core;

public interface IStateContext<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    IActionHandler<TSubject, TAction>? Get<TAction>() where TAction : IAction<TSubject>;
}