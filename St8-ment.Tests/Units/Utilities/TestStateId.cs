namespace St8Ment.Tests.Units.Utilities;

public class TestStateId
{
    public static readonly StateId New = new("NEW");
    public static readonly StateId Processing = new("PROCESSING");
    public static readonly StateId Complete = new("COMPLETE");
    public static readonly StateId Fault = new("FAULT");
}