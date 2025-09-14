namespace St8Ment.StateMachines;

public interface IStateMachineProvider<TKey>
{
    IStateMachine? Get(TKey key);
}