using System.Threading.Tasks;

namespace St8Ment.StateMachines
{
    public interface ITransitionCallback<TInput>
    {
        Task Execute(TInput action);
    }
}
