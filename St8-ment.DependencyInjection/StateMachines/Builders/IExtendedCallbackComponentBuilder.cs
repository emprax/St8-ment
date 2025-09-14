namespace St8Ment.DependencyInjection.StateMachines.Builders;

public interface IExtendedCallbackComponentBuilder<TInput>
{
    IExtendedStateComponentBuilder To(StateId stateId);
}
