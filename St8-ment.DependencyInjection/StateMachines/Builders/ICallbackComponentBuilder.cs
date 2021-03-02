namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    public interface ICallbackComponentBuilder<TInput>
    {
        IStateComponentBuilder To(StateId stateId);
    }
}
