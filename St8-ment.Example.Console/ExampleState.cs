namespace St8Ment.Example.Console
{
    public class ExampleState : StateId
    {
        public static ExampleState Start = new("START");
        public static ExampleState New = new("NEW");
        public static ExampleState Updating = new("UPDATING");
        public static ExampleState Complete = new("COMPLETE");
        public static ExampleState Published = new("PUBLISHED");
        public static ExampleState Revoked = new("REVOKED");
        public static ExampleState Fault = new("FAULT");

        private ExampleState(string name) : base(name) { }
    }
}
