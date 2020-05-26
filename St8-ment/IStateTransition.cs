using System.Threading;
using System.Threading.Tasks;

namespace St8_ment
{
    public interface IStateTransitionMarker { }

    public interface IStateTransition<in TTransaction> : IStateTransitionMarker where TTransaction : ITransaction
    {
        Task Handle(TTransaction action, CancellationToken cancellationToken);
    }
}