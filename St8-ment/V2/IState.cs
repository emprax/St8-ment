﻿using System;

namespace St8_ment.V2
{
    public interface IState { };

    public interface IState<TContext> : IState where TContext : IStateContext<TContext>
    {
        TContext Context { get; }

        IActionAccepter<TContext> Connect(IStateMachine<TContext> stateMachine);

        Type GetConcreteType();
    }
}
