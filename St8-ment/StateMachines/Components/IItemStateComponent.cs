namespace St8Ment.StateMachines.Components
{
    public interface IItemStateComponent : IStateComponent
    {
        void Add(IStateComponent component);
    }
}
