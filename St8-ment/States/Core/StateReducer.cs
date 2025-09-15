using St8Ment.States.Abstractions.Core;
using System.Threading;
using System.Threading.Tasks;

namespace St8Ment.States.Core;

public class StateReducer<TState> : IStateReducer<TState>, IStateHandle<TState> where TState : class, IStateSubject<TState>
{
    private readonly IStateContextProvider<TState> provider;

    public StateReducer(IStateContextProvider<TState> provider, TState subject)
    {
        this.provider = provider;
        this.Subject = subject;
    }

    public TState Subject { get; }

    public void Transition(StateId state) => this.Subject.State.Id = state;

    public async Task<StateRepsonse> ExecuteAsync<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction<TState>
    {
        var context = this.provider.Get(this.Subject.State.Id);
        if (context is null)
        {
            return StateRepsonse.NoState<TAction>(this.Subject.State.Id);
        }

        var handler = context.Get<TAction>();
        if (handler is null)
        {
            return StateRepsonse.NoAction<TAction>(this.Subject.State.Id);
        }

        await handler.ExecuteAsync(action, this, cancellationToken);
        return StateRepsonse.Success<TAction>(this.Subject.State.Id);
    }
}