using St8Ment.States.Abstractions.Core;

namespace St8Ment.Tests.Units.Utilities;

delegate void ActionOutputCallback<TAction, TSubject>(out IActionHandler<TSubject, TAction> handler)
    where TAction : IAction<TSubject>
    where TSubject : class, IStateSubject<TSubject>;