using St8Ment.DependencyInjection.StateMachines.Builders;

namespace St8Ment.DependencyInjection.StateMachines
{
    public interface IStateConfiguration
    {
        StateId StateId { get; }

        void Configure(IStateComponentBuilder builder);
    }
}
