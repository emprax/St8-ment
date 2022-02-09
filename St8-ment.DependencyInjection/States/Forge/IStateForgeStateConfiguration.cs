using St8Ment.States;

namespace St8Ment.DependencyInjection.States.Forge
{
    public interface IStateForgeStateConfiguration<in TSubject> where TSubject : StateSubject
    {
        StateId StateId { get; }

        void Configure(IStateForgeStateBuilder<TSubject> builder);
    }
}
