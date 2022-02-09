namespace St8Ment.States
{
    public interface IStateHandle<out TSubject> where TSubject : StateSubject
    {
        TSubject Subject { get; }

        void Transition(StateId nextState);
    }
}
