namespace St8_ment
{
    public interface IStateTransitionProvider
    {
        IStateTransitioner<TTransaction> Find<TTransaction>() where TTransaction : ITransaction;
    }
}