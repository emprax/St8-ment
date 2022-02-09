namespace St8Ment.States.Forge
{
    public interface IStateForge
    {
        IState Connect<TSubject>(TSubject subject) where TSubject : StateSubject;
    }
}