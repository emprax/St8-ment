namespace St8Ment.DependencyInjection.StateMachines.Abstractions;

public interface IExtendedStateComponentBuilder
{
    IExtendedStateTransitionBuilder<TInput> On<TInput>();

    IExtendedStateTransitionBuilder<object> OnDefault();
}
