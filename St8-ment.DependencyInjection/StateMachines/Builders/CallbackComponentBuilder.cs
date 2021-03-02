using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    internal class CallbackComponentBuilder<TInput> : ICallbackComponentBuilder<TInput>
    {
        private readonly IStateComponentBuilder builder;
        private readonly IItemStateComponent parent;

        public CallbackComponentBuilder(IStateComponentBuilder builder, IItemStateComponent parent)
        {
            this.builder = builder;
            this.parent = parent;
        }

        public IStateComponentBuilder To(StateId stateId)
        {
            this.parent.Add(new ResultComponent(stateId));
            return this.builder;
        }
    }
}
