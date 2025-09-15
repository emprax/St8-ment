namespace St8Ment.States;

public readonly struct StateRepsonse
{
    private StateRepsonse(StateResponseType type, StateId state, string action)
    {
        this.Type = type;
        this.State = state;
        this.Action = action;
    }

    public StateResponseType Type { get; }

    public StateId State { get; }

    public string Action { get; }

    public static StateRepsonse NoState<TAction>(StateId state) => new(StateResponseType.NOSTATE, state, typeof(TAction).Name);

    public static StateRepsonse NoAction<TAction>(StateId state) => new(StateResponseType.NOACTION, state, typeof(TAction).Name);

    public static StateRepsonse Success<TAction>(StateId state) => new(StateResponseType.SUCCESS, state, typeof(TAction).Name);
}