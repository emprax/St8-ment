using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateConfiguration<TSubject> where TSubject : ExtendedStateSubject<TSubject>
    {
        StateId StateId { get; }

        void Configure(IStateBuilder<TSubject> builder);
    }
}
