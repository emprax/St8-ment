﻿using St8Ment.States;

namespace St8Ment.Tests.Units.Utilities
{
    delegate void StateOutputCallback<TSubject>(StateId id, out IActionProvider<TSubject> provider) where TSubject : ExtendedStateSubject<TSubject>;

    delegate void ActionOutputCallback<TAction, TSubject>(out IActionHandler<TAction, TSubject> handler)
        where TAction : class, IAction
        where TSubject : ExtendedStateSubject<TSubject>;
}
