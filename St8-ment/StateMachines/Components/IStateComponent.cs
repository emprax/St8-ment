using System;
using System.Threading.Tasks;

namespace St8_ment.StateMachines.Components
{
    public interface IStateComponent
    {
        Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id);
    }
}
