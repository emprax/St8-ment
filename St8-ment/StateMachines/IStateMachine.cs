using System.Threading.Tasks;

namespace St8Ment.StateMachines
{
    public interface IStateMachine
    {
        StateId Current { get; }

        Task<StateMachineResponse> Apply<TInput>(TInput action);
    }
}
