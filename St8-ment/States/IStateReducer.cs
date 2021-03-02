﻿namespace St8Ment.States
{
    public interface IStateReducer<TContext> where TContext : class, IStateContext<TContext>
    {
        bool TryGetProvider(StateId id, out IActionProvider<TContext> provider);

        void SetState(StateId stateId, TContext context);
    }
}
