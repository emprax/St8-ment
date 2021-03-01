using System.Threading.Tasks;

namespace St8_ment.StateMachines
{
    public interface IStateMachine
    {
        StateId Current { get; }

        Task<StateMachineResponse> Apply<TInput>(TInput action);
    }
}
