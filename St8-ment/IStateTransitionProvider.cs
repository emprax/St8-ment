namespace St8_ment
{
    public interface IStateTransitionProvider
    {
        IStateTransition<TTransaction> Find<TTransaction>() where TTransaction : ITransaction;
    }
}