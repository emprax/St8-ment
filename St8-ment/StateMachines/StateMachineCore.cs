using St8Ment.StateMachines.Components;

namespace St8Ment.StateMachines;

public record StateMachineCore(StateId InitialStateId, IStateComponent Component) : IStateMachineCore;
