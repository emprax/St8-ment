using St8_ment.StateMachines.Components;

namespace St8_ment.StateMachines
{
    public interface IStateMachineCore
    {
        StateId InitialStateId { get; }

        IStateComponent Component { get; }
    }
}
