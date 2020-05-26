using System.Threading;
using System.Threading.Tasks;

namespace St8_ment
{
    public interface IStateContext
    {
        Task<bool> Apply<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction;
    }
}
