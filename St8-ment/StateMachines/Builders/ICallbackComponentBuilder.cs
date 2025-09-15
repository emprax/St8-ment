namespace St8Ment.StateMachines.Builders;

public interface ICallbackComponentBuilder<TInput>
{
    IStateComponentBuilder To(StateId stateId);
}
