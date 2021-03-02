using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateConfiguration<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        StateId StateId { get; }

        void Configure(IStateBuilder<TSubject> builder);
    }
}
