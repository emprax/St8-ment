using System;
using System.Threading.Tasks;

namespace St8Ment.StateMachines.Components
{
    public interface IStateComponent
    {
        Task<StateTransitionResponse> Apply<TInput>(TInput input, StateId id);
    }
}
