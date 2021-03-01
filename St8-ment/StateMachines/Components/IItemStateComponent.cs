namespace St8_ment.StateMachines.Components
{
    public interface IItemStateComponent : IStateComponent
    {
        void Add(IStateComponent component);
    }
}
