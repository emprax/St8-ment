using St8Ment.StateMachines.Builders;

namespace St8Ment.StateMachines;

public interface IStateConfiguration
{
    StateId StateId { get; }

    void Configure(IStateComponentBuilder builder);
}