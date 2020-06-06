using System;

namespace St8_ment.V2
{
    public abstract class State<TSelf, TContext> : IState<TContext>
        where TSelf : IState<TContext>
        where TContext : IStateContext<TContext>
    {
        protected State(TContext context) => this.Context = context;

        public TContext Context { get; }

        public Type GetConcreteType() => typeof(TSelf);
    }
}
