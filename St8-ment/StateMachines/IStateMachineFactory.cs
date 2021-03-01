namespace St8_ment.StateMachines
{
    public interface IStateMachineFactory<TKey>
    {
        IStateMachine Create(TKey key);
    }
}