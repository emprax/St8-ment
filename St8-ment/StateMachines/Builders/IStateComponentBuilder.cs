namespace St8Ment.StateMachines.Builders;

public interface IStateComponentBuilder
{
    IStateTransitionBuilder<TInput> On<TInput>();

    IStateTransitionBuilder<object> OnDefault();
}