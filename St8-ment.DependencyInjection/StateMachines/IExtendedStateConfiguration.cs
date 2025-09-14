using St8Ment.DependencyInjection.StateMachines.Builders;

namespace St8Ment.DependencyInjection.StateMachines;

public interface IExtendedStateConfiguration
{
    StateId StateId { get; }

    void Configure(IExtendedStateComponentBuilder builder);
}