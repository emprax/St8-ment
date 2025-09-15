using St8Ment.States.Core;

namespace St8Ment.States.Abstractions.Core;

public interface IStateSubject<TSelf> where TSelf : class, IStateSubject<TSelf>
{
    State State { get; }
}