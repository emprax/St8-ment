namespace St8_ment.V1
{
    public interface IStateTransitionerProvider
    {
        IStateTransitioner<TTransaction> Find<TTransaction>() where TTransaction : ITransaction;
    }
}