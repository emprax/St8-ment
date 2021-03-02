# <img src=".\docs\St8-ment-logo.png" width ="200" height="200" /> St8-ment

![Nuget](https://img.shields.io/nuget/v/St8-ment?color=green&style=plastic)

NuGet package pages:
- [St8-ment](https://www.nuget.org/packages/St8-ment/)
- [St8-ment.DependencyInjection](https://www.nuget.org/packages/St8-ment.DependencyInjection/)

## Introduction

A more flexible state/state-machine pattern library for SOLID state pattern design. Achieved by separating the state-object and the action + transitioning system into individual components. These transitions can be viewed as **request-handlers** in a request-to-handler model (like modern library such as **MediatR**). They respond to the input of an action and determine how a state transitions into other states.

Previously, the library provided a V1 and V2 version. However, because of some new insights into the matter, the whole library has gained an total overhaul. Use a version before 2.0.0 to use of the previous setup. The [documentation](./docs/README(old).md) regarding that setup has been relocated to the /docs section.

This version of the documentation will concern the overhauled version of the St8-ment library.

## Table of content

- [St8-ment](#st8-ment)
  * [The problem](#the-problem)
  * [A solution](#a-solution)
    + [V1](#v1)
    + [V2](#v2)
  * [How it works](#how-it-works)
    + [V1](#v1-1)
    + [V2](#v2-1)
  * [Coding Guide](#coding-guide)
    + [Context](#context)
      - [V2](#v2-2)
    + [Transitioners](#transitioners)
      - [V2](#v2-3)
    + [Actions](#actions)
    + [States](#states)
      - [V2](#v2-4)
    + [Registration and usage](#registration-and-usage)
      - [V2](#v2-5)
  - [Reasons for using this library](#reasons-for-using-this-library)

<small><i><a href='http://ecotrust-canada.github.io/markdown-toc/'>Table of contents generated with markdown-toc</a></i></small>

## The problem

In the original state-pattern, the state objects (polymorphic implementations of a common state interface) contain the logic for state-transitions but also provide the same interface (in regards of methods/actions) as its subject (sometimes called context). This means that when the subject changes its interface (class-structure), the state objects also have to be refactored. Furthermore, the state objects themselves also determine into which state they transition next when a certain action is applied. All this logic is fairly simple to apply and provides robust state guarding and transitioning. However, the extendibility and hardcoded logic both make it difficult to change certain aspects without re-implementing the whole state-object (or state-objects when the subject changes).

![state](docs/standard-state.png)

Next to that, the amount of responsibilities is increased, the abstraction for the those objects are growing as well and most configurations require the objects to communicate with one-another by the means of their concrete implementations instead of an abstraction. So altogether this pattern can cause some problems in regards to violating the SOLID design principles, although this is mostly accepted as this pattern is generally considered to be quite a well fitting solution. Nevertheless, this library tries to solve the aforementioned issues and takes the inspiration from multiple other initiatives like, i.e., the Redux Pattern + the StateMachine Pattern. 

## A solution

This library provides a proposed solution for the aforementioned problem. St8-ment provides to state-based pattern implementations, **state** and **state-machine**. The state version is a direct solution to aforementioned the state-pattern, whereas the state-machine pattern is a dedicated state-machine implementation. Both are designed with modern technology in mind and comes with **dependency injection** extensions for *Microsoft ServiceCollection*. 

The implementations are based on modern **request + request-handler** style solutions with makes it easier to use dependencies.

### State

The state-pattern based solution in this library uses a so-called state-reducer to create new states regarding a state-id and the subject/context. The state-objects refer to the reducer to get their corresponding action-providers which provides the action-handlers for a particular action that are registered on the respected state. These resulting action-handler objects are stored in key-value stores within the state-specific action-providers where the actions function as keys. When a state does contain an action-handler for a specific action, then that actions can be applied a possible transition into a new state could occur (that dependents on what the developer of that particular action-handler has in mind).

The actions (and corresponding actions-handlers) can be registered to a particular state by the means of the aforementioned dependency injection extensions.

**NOTE:** What is displayed in figure below is the belonging-to-hierarchy. The actual flow works a bit different and will be explain further down below in the section about the workings of the library components.

<img src="docs/St8-ment-state.png" alt="st8-ment" width ="700" height="500" />

**NOTE:** Be aware that an action-handler object does not always handle a single state transition as it more or less an equivalent to the effects from the Redux libraries. They contain the state-logic like the methods in the standard state-pattern. Therefore they can make a decision to transition into a specific state or, when a specific condition fails for example, they could determine to transition into a different states. They could even choose to not transition into another state at all when no conditions are met. The subject/context will then stay in its current state. But these choices lay in the hands of the developers that create and manage their specific action-handlers.*

### State-machine

The state-machine version in this library is setup differently in respect to the state-based version. The state-machine version provides a transitions into new states controlled by the type of input that is provided to the state-machine. Every transition can have its own guard (specification by **SpeciFire** library) and transition-callback, which are both completely optional but are recommended nonetheless. Every state has its own set of inputs it can react to, these can be registered to the state-machine by the aforementioned dependency injection extensions.

One specific difference in regards to the state-based version is that the transitions in the state-machine are predefined regarding into which new state the machine transitions. 

**NOTE:** *When a callback is execute and throws an exception, then this exception will be caught and provided in a specific result, however, the transition into the targeted state will no longer occur at that moment.*

## How it works

### State

The diagram shown below is there to help creating an understanding of the system and it bears quite some similarity to the Redux diagram that can be found online. The numbers in the diagram refer to the steps that are described below the diagram. 

<img src="docs/St8-ment-diagram-state.png" alt="st8-ment" width ="700" height="770" />

**Steps:**

1. The context (in most cases a data-model, system or an aggregate-root (DDD)) can hold a state and has an *AcceptAction(...)* method that accepts an action. 
2. The action is transmitted it to the state accepted through its own *Accept* method.
3. The incoming action is being verified by the state, to determine whether it has an action-handler for it. This verification is achieved by determining whether the action-provider (retrieved from the state-reducer) actually contains an action-handler for this action. **NOTE:** The action-providers are stored in a special storage core in the reducer per state-id, so when a state wants to consult its corresponding action-handler, it should do so by querying the state-reducer. However, it could be the case that there is no action-handler for the respected state, hence a special state-response is returned to notify the caller of the accept method. A same sort of process will occur when an action cannot be mapped to a corresponding action-handler from the retrieved action-provider.
4. When the action-handler is successfully retrieved, it will be prompted to execute its logic regarding the provided action together with a state-view that contains the current state-id and the context. When the process succeeds, the state will then again consult the state-reducer by calling its *SetState(...)* method. This method needs both the target state-id and the subject/context.
5. The state is then set to the context and the cycle is complete. **NOTE:** When initializing the subject/context, it is a good idea to call the state-reducer (typed for that context) and utilize the *SetState* method to setup the context with an initial state.

### State-machine

Again a Redux-diagram-like diagram is used to visualize the steps for the state-machine based solution.

<img src="docs/St8-ment-diagram-state-machine.png" alt="st8-ment" width ="615" height="800" />

**Steps:**

1. An input applied to the state-machine.

2. The state-machine determines whether it has registered state transitions that adhere to the type of the input in combination with the current state.

3. When the corresponding transition is found, a few actions can be applied in order (otherwise see step **3.A.**):

   - (Optional) When there is a specification (implementation with a ISpec<T> from the **SpeciFire** library) as guard available, it will be called to check whether the input adheres to the specification. When the input does not adhere to the specification, this step will return a not-satisfied response, further actions after this case will be discussed in section **3.A**. 
     - When there is no guard available, this step will be skipped.
   - (Optional) When there a transition-callback available, it will be called to execute the callback operation. When the callback throws an exception, then this exception will be caught and this step will return an exception-response, further actions after this case will be discussed in section **3.A**. 
     - When there is no transition-callback available, this step will be skipped.
   - The final step in this block is to return a new state-id (the ID of the state that it should transition into).

   **A. Important:** When the registration of the state is provided with a default case, the failed steps, mentioned directly above, will at least return to the state component which will try to execute the default transition. This is a convenient feature for tidying up the failed cases. For example, by always transitioning into a fault-state when a failure occurs.

4. The retrieved state (ID) will be returned to the state-machine.
5. The state-machine will update its state by setting the new state-id.

## Coding Guide

This section emphasizes the important components of the library on behalf of some coding examples. Starting with the definition of some of the components in regard to their purpose and location within an application. At first the V1 version will be discussed and then the V2 will be compared to the V1 version per section.

### Context

#### State

The context contains the state, although, it is not explicitly required by the interface. The reason for this is that the state is not directly available by default, it has to be set by the state-reducer by means of the *SetState(...)* method. The context delegates the determination of which actions are allow on current state of the context to the state itself, but also to transition into another state when specified.

```c#
public class Order : IAggregateRoot, IStateContext<Order>
{
    public IState<Order> State { get; private set; }
    
    public Task<bool> AcceptAction<TAction>(TAction action) where TAction : class, IAction
    {
        //...........
        return State.Accept(action);
    }
    
    public void SetState(IState<Order> state)
    {
     	this.State = state;
    }
    
    //...........
}
```

The *Accept(...)* and *SetState(...)* methods are the to be implemented methods regarding the IStateContext<TContext> interface.

### Transitioners

To implement the state transitioners, the intention is to implement the StateTransitioner abstract-class instead of the IStateTransitioner interface, as the abstraction applies some specific generic operations that are applied in the background. 

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

As you can see the transitioner can handle multiple specific paths of transitioning into different states regarding specific conditions. Notice that the StateTransitioner has three generics. The first one regards the current state, the second the context and the third one is the action it is supposed handle.

#### V2

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

The IStateTransitioner interface can now be used directly. The StateMachine is no longer needed here as it has gained a more prominent role up front in the V2 version. Data transactions from one state to the other are also much more flexible.

### Actions

The action object itself is not that interesting as it is more or less only a label in object form that can also contain some specific action-related data. For example:

```C#
public class CancleOrderAction : IAction { }
```

In this case there is no specific data displayed, but there are no rules against it, so feel free to add specific action-related data like what is similar to other commonly used request-like objects. 

The V2 version of the actions are the same.

### States

The state objects are implemented by extending the State<TSelf, TContext> abstract class, which contains the IStateTransitionerProvider. The transitioner provider is used to determine the transitioner to use in regards to the provided action. When a transitioner for the specified action cannot be found, it will simply return false to indicate the transaction failed. 

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

This NewOrderState is an implementation of the State object. It provides its own type with the TSelf generic of the State and the Order as context with the TContext generic. The GetSelf method returns the TSelf initialized object for usage in other cases. The state can contain specific data in regard to the relation between the context and that specific state.

#### V2

Again, like the transitioners, the V2 version of the states are a bit different. The State<TSelf, TContext> abstract class is still used but with a varied setup. The GetSelf method is no longer needed to provide the state as the state can now be created simply by calling its constructor. In the V1 version the states had a more prominent role with dependencies and had to be registered in the DI, while this role is no longer that prominent for version V2. Next to that, the V1 state needed to provide itself to the DI construction to simply construct itself, while as with the V2 version this is no longer the case.

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

Nevertheless, the State abstraction still contains a Connect method, that accepts a StateMachine. The StateMachine, used here as visitor, uses the State object type to create the right abstraction.

### Registration and usage

A new IServiceCollection extension is used to connect all the aforementioned components. It is provided by the St8-ment.DependencyInjection binary. A whole set of builders, appliers and other constructions are provided by this binary as well, but these are purely used by the AddStateMachine extension and have therefore not a direct usage for the user/developer.

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

A few things can be observed within this section. The configuration for a specific state can be created by using the For method on the builder, followed by either a lambda-expression that registers all transitioners or by a custom configuration that extends the StateConfiguration abstract class. At last, there is the option to register no configuration for a state at all. This is necessary for assuring that the state-machine knows that particular state, but doesn't have any actions for it, effectively creating a *sink* state.

An example of a custom state-configuration would look like this:

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

Now, when everything is registered, the system can be used in software solutions. 

```c#
var order = Order.Create(...);
stateMachine.Apply<NewOrderState>(order);

......

// The first state is NewOrderState which can transition into the CheckedOrderState.
await order.Accept(new CheckOrderAction(), CancellationToken.None);	 

// The CheckedOrderState can transition into the DeliveredOrderState.
await order.Accept(new DeliverOrderAction(), CancellationToken.None);

// The DeliveredOrderState can transition into the CompletedOrderState.
await order.Accept(new CompleteOrderAction(), CancellationToken.None);
```

#### V2

The V2 version of the registrations for the StateMachines is near identical to the V1 version. The use of the StateMachine however, is a bit different.

```c#
var order = Order.Create(...);
order.SetState(new NewOrderState(order));

......

// The first state is NewOrderState which can transition into the CheckedOrderState.
await order.State
    .Connect(stateMachine)
    .Apply(new CheckOrderAction());
    
// The CheckedOrderState can transition into the DeliveredOrderState.
await order.State
    .Connect(stateMachine)
    .Apply(new DeliverOrderAction());

// The DeliveredOrderState can transition into the CompletedOrderState. Note the use of the StateContextExtensions Apply method.
await order.Apply(stateMachine, new CompleteOrderAction());
```

First the state in the context is set to a new order (note that this could also be integration in a more intuitive way, but it depends on the choices of the developer. The simple, but still a bit specific, use of the SetState method is shown here to illustrate an example of usage). The state in the context is visited by the StateMachine through the Connect method of the state. The StateMachine retrieves some specific state type setup and can then retrieve the right TransitionerProvider by providing a specific ActionAccepter. The action is applied to the accepter and the state system is set in motion. 

Note the use of a specific extension-method called *Apply*, what is an extension of the IStateContext and encapsulates the aforementioned State - StateMachine with Action interaction.



## Reasons for using this library

This library is useful for a more dynamic and solidified state pattern design. Note that the state pattern is already known for being quite a suitable design pattern in multiple solutions. Therefore this library provides even more SOLID design in state usage. The library has quite some similarities to the Redux library, but it is still its own take on the appliance of state and the patterns that are related to that. Hence this library is more or less a combination of the state pattern and state-machine pattern with some Redux influences. So, when wanting to use a SOLID designed state transitioning system for your software, where that is on par with front-end libraries like Redux and NGRX, then this library is a great choice.
