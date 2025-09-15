namespace St8Ment.Tests.Integration.Utilities;

public class TestStateId
{
    public static readonly StateId New = new("NEW");
    public static readonly StateId Processing = new("PROCESSING");
    public static readonly StateId Complete = new("COMPLETE");
    public static readonly StateId Fault = new("FAULT");
}