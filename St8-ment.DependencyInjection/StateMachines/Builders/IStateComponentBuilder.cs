namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    public interface IStateComponentBuilder
    {
        IStateTransitionBuilder<TInput> On<TInput>();

        IStateTransitionBuilder<object> OnDefault();
    }
}
