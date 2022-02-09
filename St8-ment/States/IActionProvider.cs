namespace St8Ment.States
{
    public interface IActionProvider<TSubject> where TSubject : StateSubject
    {
        bool TryGet<TAction>(out IActionHandler<TAction, TSubject> actionHandler) where TAction : class, IAction;
    }
}
