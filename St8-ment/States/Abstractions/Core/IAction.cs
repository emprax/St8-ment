namespace St8Ment.States.Abstractions.Core;

public interface IAction<TSubject> where TSubject : class, IStateSubject<TSubject>;