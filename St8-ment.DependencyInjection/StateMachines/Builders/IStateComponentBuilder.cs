namespace St8_ment.DependencyInjection.StateMachines.Builders
{
    public interface IStateComponentBuilder
    {
        IStateTransitionBuilder<TInput> On<TInput>();

        IStateTransitionBuilder<object> OnDefault();
    }
}
