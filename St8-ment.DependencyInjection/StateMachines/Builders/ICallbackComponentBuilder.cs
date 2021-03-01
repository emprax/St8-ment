namespace St8_ment.DependencyInjection.StateMachines.Builders
{
    public interface ICallbackComponentBuilder<TInput>
    {
        IStateComponentBuilder To(StateId stateId);
    }
}
