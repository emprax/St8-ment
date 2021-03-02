namespace St8Ment.Example.Console
{
    public class ExampleState : StateId
    {
        public static ExampleState Start = new ExampleState("START");
        public static ExampleState New = new ExampleState("NEW");
        public static ExampleState Updating = new ExampleState("UPDATING");
        public static ExampleState Complete = new ExampleState("COMPLETE");
        public static ExampleState Published = new ExampleState("PUBLISHED");
        public static ExampleState Revoked = new ExampleState("REVOKED");
        public static ExampleState Fault = new ExampleState("FAULT");

        private ExampleState(string name) : base(name) { }
    }
}
