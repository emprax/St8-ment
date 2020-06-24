using System;

namespace St8_ment.V2
{
    public abstract class State<TSelf, TContext> : IState<TContext>
        where TSelf : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        protected State(TContext context) => this.Context = context;

        public TContext Context { get; }

        public Type GetConcreteType() => typeof(TSelf);

        public IActionAccepter<TContext> Connect(IStateMachine<TContext> stateMachine)
        {
            return new ActionAccepter<TContext, TSelf>(stateMachine.For(this as TSelf));
        }
    }
}
