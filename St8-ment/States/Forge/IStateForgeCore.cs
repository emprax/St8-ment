namespace St8Ment.States.Forge
{
    public interface IStateForgeCore
    {
        IStateCore GetForState(StateId id);
    }
}