﻿namespace St8_ment.Example.Console
{
    public class InitialState : State<InitialState, ExampleContext>
    {
        public InitialState(ExampleContext context, IStateTransitionerProvider provider) : base(context, provider)
        {
            this.Name = "The initial state";
        }

        public string Name { get; }

        protected override InitialState GetSelf() => this;
    }
}
