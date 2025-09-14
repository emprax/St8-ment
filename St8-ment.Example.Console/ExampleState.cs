namespace St8Ment.Example.Console;

public static class ExampleState
{
    public readonly static StateId Start = new("START");
    public readonly static StateId New = new("NEW");
    public readonly static StateId Updating = new("UPDATING");
    public readonly static StateId Complete = new("COMPLETE");
    public readonly static StateId Published = new("PUBLISHED");
    public readonly static StateId Revoked = new("REVOKED");
    public readonly static StateId Fault = new("FAULT");
}
