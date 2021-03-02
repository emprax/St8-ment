using St8Ment.StateMachines.Components;

namespace St8Ment.StateMachines
{
    public class StateMachineCore : IStateMachineCore
    {
        public StateMachineCore(StateId initialStateId, IStateComponent component)
        {
            this.InitialStateId = initialStateId;
            this.Component = component;
        }

        public StateId InitialStateId { get; }

        public IStateComponent Component { get; }
    }
}
