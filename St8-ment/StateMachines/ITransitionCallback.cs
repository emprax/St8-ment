using System.Threading.Tasks;

namespace St8_ment.StateMachines
{
    public interface ITransitionCallback<TInput>
    {
        Task Execute(TInput action);
    }
}
