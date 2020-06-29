# St8-ment
A dynamic state/state-machine pattern library for SOLID state pattern design. This is achieved by separating the state-object and the action + transitioner into separate components whereas the transitioners are considered to be request-handlers in a request-to-handler model. They respond to the input of an action and determine how a state transitions into other states. An action and the state-object itself are combined in a transaction which is the actual request model. There the actions are labels and models at the same time that hold the data for the requests.

The library provides a V1 and V2 version. Where the V1 is more closely modelled after the State Design Pattern, whereas the V2 version is more or less the StateMachine version of this pattern. The V2 focusses less on the behavior in the state itself and by this provides the possibility to use simple mapping from 1 state to another in the provide transitioners. The V1 version, however, does not provide this possibility as the state holds the logic to determine which transitioners to use as well as that the context should at that point be more responsible for the data. The V2 provides more freedom as the state object now has also more data holding possibilities.

### The problem

The transitioners hold the state-logic that is usually housed in the state-object itself, when regarding the standard State Pattern, but is confined to the amount of methods a state holds. The state in this case usually contains the same methods as the context it is considered to be the state for. So extending the amount of operations that require state specific transitions can only be achieved by modifying the class-structure of both the state and the context. 

![state](docs/standard-state.png)

Next to that the amount of responsibilities is increased, the abstraction for the those objects are growing as well and most configurations require the objects to communicate with one-another by the means of their concrete implementations instead of an abstraction. So altogether this pattern can cause some problems in regards to violating the SOLID design principles, although this is mostly accepted as this pattern is considered to be quite a well fitting solution. This library tries to solve the aforementioned issues and takes the inspiration from multiple other initiatives like, i.e., the Redux Pattern. 

### A solution

As prompted before, there is a V1 and V2 version of the code solution, both with their own insights.

#### V1

The library consists of state-machines that provide and create the state-objects for a specific context each. The state-objects refer to a provider/registry which stores all the different transitioner-objects from that state, where these transitioner-objects are stored in a key-value store by which the actions function as keys. When a state does contain a transitioner-object for a specific action, then that transitioner can be applied.

![state-machine](docs/St8-ment-state.png)

**Note:** The transitioner-objects are called transitioners because of there purpose/behavior, but be aware that a transitioner-object does not always handle a single state transition as it more or less an equivalent to the effects from the Redux libraries. They hold the state-logic like the methods in the standard state-pattern and can therefore make a decision to transition to one state or, when a specific condition fails for example, it could determine to transition to a different state or even none at all when no conditions are met and stay in their current state. But these choices lay in the hands of the developer of specific transitioners.

#### V2

The V2 version differs from V1 in a sense that the order/hierarchy of the components referring to one another are different. Where in the V1 version the State has the foreground, there the V2 version gives that spot to the StateMachine. The context provides its state to the StateMachine by which an action accepter accepts an action. The StateMachine holds the providers for the transitioners and the action is again used to find the matching transaction into the right state transitioner.

### How it works

#### V1

The diagram shown below is there to support the understanding of the system and it bears quite some similarity to the Redux diagram that can be found quite easily online. The numbers in the diagram refer to the described steps. **Note:** A separate path that can be taken and is determined by condition. It is not shown as a number but will be described in the steps.

![st8-ment](docs/St8-ment-diagram.png)

**Steps:**

1. The context (mostly a system or aggregate-root) contains the state and an accept method that accepts an action. 
2. The actions is transported to the state and accepted by the state its own accept method.
3. The incoming action is being verified by the state, to determine whether there is a transitioner related to that action. This verification is achieved by determining whether the TransitionerProvider actually contains a transitioner for this action.
4. When this is the case, the chosen transitioner executes its logic by which it can then use the state-machine to choose the next state. 
5. The state is eventually set to the context and the cycle is complete. But when the transitioner cannot be found, a boolean determining the success of the state transition operation returned by the accepting methods to be false and no state changes have been made.

#### V2

For V2, there are quite some changes as what was already described in the previous section.

<img src="docs/St8-ment-diagram-V2.png" alt="st8-ment" style="zoom:47%;" />

**Steps:**

1. The context (again, mostly a system or aggregate-root) contains the state.
2. The state is provided to the StateMachine which determines the right provider of transitioners for that specific state. Be aware that the StateMachine is used like a visitor here and it visits the state by a special Connect method, from there further preparations can be done.
3. The TransitionerProvider is being encapsulated in an ActionAccepter and can apply an action on it.
4. The incoming action is being verified by the ActionAccepter, to determine whether there is a transitioner related to that action. This verification is achieved by determining whether the TransitionerProvider actually contains a transitioner for this action.
5. When this is the case, the chosen transitioner executes its logic by which it can then change to a next state, this makes it transition friendlier than the V1 version as the data can now be past from one state to another. 
6. The state is eventually set to the context and the cycle is complete. But when the transitioner cannot be found, a boolean determining the success of the state transition operation returned by the accepting methods to be false and no state changes have been made.

### Coding Guide

This section emphasizes the important components of the library on the basis of some coding examples. We first start with the definition of some of the components in regard to their purpose and location within an application. At first the V1 version will be discussed and then the V2 will be compared to V1 per section.

#### Context

The context is the state holder. It uses the state to determine which actions are allowed and how to transition to another state, by the means of the transitioners, when a certain action is being issued. 

```c#
public class Order : IAggregateRoot, IStateContext<Order>
{
    public IState<Order> State { get; private set; }
    
    public Task<bool> Accept<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction
    {
        //...........
        return State.Accept(action, cancellationToken);
    }
    
    public void SetState<TState>(TState state) where TState : class, IState<ExampleContext>
    {
        //...........
     	this.State = state;   
    }
    
    //...........
}
```

The *Accept(...)* and *SetState(...)* methods are the to be implemented methods regarding the IStateContext<TContext> interface.

##### V2

The V2 version of the context looks nearly the same, but differs in a specific detail. The Accept method is removed in this version as the context purely holds the state and the state is no longer the main command-center as it is more similar to a status and data holding object that can be used in combination with an action to form a transaction to a transitioner.

```c#
public class Order : IAggregateRoot, IStateContext<Order>
{
    public IState<Order> State { get; private set; }
    
    public void SetState<TState>(TState state) where TState : class, IState<ExampleContext>
    {
        //...........
     	this.State = state;   
    }
    
    //...........
}
```



#### Transitioners

To implement the state transitioners it is the intention to implement the StateTransitioner abstract-class instead of the IStateTransitioner interface, as the abstraction provides just a bit more clear processing details than what the interface is providing. 

```c#
public class CancelOrderStateTransitioner : StateTransitioner<ProcessedOrderState, Order, CancelOrderAction>
{
    private readonly ISpecificationFactory factory;
    
    public CancelOrderStateTransitioner(
        IStateMachine<ExampleContext> stateMachine, 
        ISpecificationFactory factory) : base(stateMachine) 
    {
        this.factory = factory;
    }
    
    protected override async Task Transition(
        StateTransaction<CancelOrderAction, ProcessedOrderState> transaction, 
        IStateMachine<Order> stateMachine, 
        CancellationToken cancellationToken)
    {
        var specification = factory.Obtain<ProcessedOrderState>(Specification.CanCancelOrderSpec);
        if (specification?.IsSatisfiedBy(transaction.State) ?? false)
        {
            await specification.Handle(transaction.State);
            stateMachine.Apply<CancelledOrderState>(transaction.State.Context);
            return;
        }
        
		// Do nothing or change state to a specific different state that handles a failed attempt to cancel the order.
    }
}
```

As you can see the transitioner can handle multiple specific paths of transitioning into different states by specific conditions. Notice that the StateTransitioner has three generics that have to be set. The first one regards the current state, the second the context and the third one the action it is supposed handle.

##### V2

The V2 version of the transitioner has been drastically changed as the StateTransitioner abstract class is no longer needed to fulfill the right setup.

```C#
public class CancelOrderStateTransitioner : IStateTransitioner<ProcessedOrderState, Order, CancelOrderAction>
{
    private readonly ISpecificationFactory factory;
    
    public CancelOrderStateTransitioner(ISpecificationFactory factory) => this.factory = factory;
    
    public async Task Transition(IStateTransaction<CancelOrderAction, ProcessedOrderState> transaction)
    {
        var specification = factory.Obtain<ProcessedOrderState>(Specification.CanCancelOrderSpec);
        if (specification?.IsSatisfiedBy(transaction.State) ?? false)
        {
            await specification.Handle(transaction.State);
            transaction.State.Context.SetState(new ProcessingState(transaction.State.Context)
            {
                Data = transaction.State.Data
            });
            
            return;
        }
        
		// Do nothing or change state to a specific different state that handles a failed attempt to cancel the order.
    }
}
```

The IStateTransitioner interface can now be used directly. The StateMachine is no longer needed hear as it has gained a more prominent role in the V2 version. Data transaction from one state to the other is also much more intuitive.

#### Actions

The action object itself is not that interesting as it is more or less only a label as object that can also contain some specific action-related data. For example:

```C#
public class CancleOrderAction : IAction { }
```

In this case there is no specific data, but there are no rules against it, so feel free to add specific action-related data like what is similar to other commonly used request-like objects. 

The V2 version of the actions are still the same.

#### States

The state objects are implemented with by extending the State<TSelf, TContext> abstract class, which contains the IStateTransitionerProvider that it uses to determine the transitioner to use in regards to the provided action. When there is no transitioner for the specified action, it will simply return false to indicate the transaction failed. 

```C#
public class NewOrderState : State<NewOrderState, Order>
{
    public NewOrderState(ExampleContext context, IStateTransitionerProvider provider) : base(context, provider)
    {
        this.Name = "A new order has been created";
    }

    public string Name { get; }

    protected override NewOrderState GetSelf() => this;
}
```

This NewOrderState is an implementation of the State object. It provides its own type to the TSelf generic of the State and the Order as context to the TContext generic. The GetSelf method returns the TSelf initialized object for usage in other cases. The state can contain specific data in regard to the relation between the context and that specific state.

##### V2

Again, like the transitioners, the V2 version of the states are quite a bit different. The State<TSelf, TContext> abstract class is still used but is setup quite a bit differently. The GetSelf method is no longer needed to provide the state as the state can no be created simply creating a new one in the transitioner, as where the states have a more prominent role with dependencies and had to be registered in the DI. Next to that, the state needed to provide itself to the DI construction to simply construct itself, while as with the V2 this is no longer the case.

```c#
public class NewOrderState : State<NewOrderState, Order>
{
    public NewOrderState(ExampleContext context) : base(context)
    {
        this.Name = "A new order has been created";
    }

    public string Name { get; }
}
```

Nevertheless, the State abstraction still contains a method Connect, that accepts a StateMachine. The StateMachine, used here as visitor, uses the State object type setup to create the right abstraction

#### Registration and usage

A new IServiceCollection extension is used to connect all the aforementioned components. It is provided by the St8-ment.DependencyInjection binary. A whole set of builders, appliers and other constructions are provided by this binary as well, but these are purely used by the AddStateMachine extension.

```C#
services.AddStateMachine<Order>(builder => 
{
    builder.For<NewOrderState>(configurator => configurator.On<CheckOrderAction>().Transition<CheckNewOrderTransitioner>());
    builder.For<CheckedOrderState>(configurator =>
    {
        configurator.On<RemoveOrderAction>().Transition<RemoveCheckedOrderTransitioner>());
        configurator.On<DeliverOrderAction>().Transition<DeliverCheckedOrderTransitioner>());
        configurator.On<FailedOrderAction>().Transition<FailedCheckedOrderTransitioner>());
    });
    builder.For<DeliveredOrderState>(new DeliveredOrderStateConfiguration());
    builder.For<RemovedOrderState>();
    builder.For<FailedOrderState>();
    builder.For<CompletedOrderState>();
});
```

A few things can be observed within this section. The configuration for a specific state is achieved by using the For method on the builder, followed by either a lambda that registers all transitioners or by a custom configuration that extends the StateConfiguration abstract class. The last option is to register nothing for a state which is necessary for assuring that the state-machine knows that state, but doesn't have any actions for it.

The custom state-configurations would for example look like this:

```C#
public class DeliveredOrderStateConfiguration : StateConfiguration<DeliveredOrderState, Order>
{
    protected override void Configure(IStateConfigurator<TState, TContext> configurator)
    {
        configurator
            .On<CancelOrderAction>()
            .Transition<CancelDeliveredOrderTransitioner>();
        
        configurator
            .On<FailedOrderAction>()
            .Transition<FailedDeliveredOrderTransitioner>();
        
        configurator
            .On<CompleteOrderAction>()
            .Transition<CompleteDeliveredOrderTransitioner>();
    }
}
```

Now, when it is registered, the system can be used in software solutions. 

```c#
var order = Order.Create(...);
stateMachine.Apply<NewOrderState>(context);

......

// The first state is NewOrderState which can transition into the CheckedOrderState.
await context.Accept(new CheckOrderAction(), CancellationToken.None);	 

// The CheckedOrderState can transition into the DeliveredOrderState.
await context.Accept(new DeliverOrderAction(), CancellationToken.None);

// The DeliveredOrderState can transition into the CompletedOrderState.
await context.Accept(new CompleteOrderAction(), CancellationToken.None);
```

##### V2

The V2 version of the registrations for the StateMachines is near identical to the V1 version. The use of the StateMachine however, is a bit different.

```c#
var order = Order.Create(...);
order.SetState(new NewOrderState(order));

......

// The first state is NewOrderState which can transition into the CheckedOrderState.
await context.State
    .Connect(stateMachine)
    .Apply(new CheckOrderAction());
    
// The CheckedOrderState can transition into the DeliveredOrderState.
await context.State
    .Connect(stateMachine)
    .Apply(new DeliverOrderAction());

// The DeliveredOrderState can transition into the CompletedOrderState. Note the use of the StateContextExtensions Apply method.
await context.Apply(stateMachine, new CompleteOrderAction());
```

First the state in the context is set to a new order (note that this could also be integration in a more intuitive way, but depends on the choices of the developer. The simple, but still a bit specific, use of the SetState method is being shown here to illustrate an example of usage). The state in the context is being visited by the StateMachine through the State its Connect method. The StateMachine retrieves some specific state type setup and can then retrieve the right TransitionerProvider by applying a specific ActionAccepter. The action is being applied and the system is set in motion. 

Note the use of a specific extension-method called Apply, what is an extension to the IStateContext and encapsulates the aforementioned State - StateMachine - Action interaction.