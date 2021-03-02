namespace St8Ment.StateMachines
{
    public interface IStateMachineFactory<TKey>
    {
        IStateMachine Create(TKey key);
    }
}