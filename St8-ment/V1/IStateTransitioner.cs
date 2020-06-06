using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.V1
{
    public interface IStateTransitioner<in TTransaction> : IStateTransitionerMarker where TTransaction : ITransaction
    {
        Task Handle(TTransaction action, CancellationToken cancellationToken);
    }
}