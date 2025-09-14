namespace St8Ment.DependencyInjection.StateMachines.Abstractions;

public interface IExtendedCallbackComponentBuilder<TInput>
{
    IExtendedStateComponentBuilder To(StateId stateId);
}
