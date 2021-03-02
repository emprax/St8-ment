using St8Ment.StateMachines.Components;

namespace St8Ment.StateMachines
{
    public interface IStateMachineCore
    {
        StateId InitialStateId { get; }

        IStateComponent Component { get; }
    }
}
