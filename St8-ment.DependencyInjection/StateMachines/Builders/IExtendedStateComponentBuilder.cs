namespace St8Ment.DependencyInjection.StateMachines.Builders;

public interface IExtendedStateComponentBuilder
{
    IExtendedStateTransitionBuilder<TInput> On<TInput>();

    IExtendedStateTransitionBuilder<object> OnDefault();
}
