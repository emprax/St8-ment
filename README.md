# St8-ment
A dynamic state/state-machine pattern library for SOLID state pattern design. This is achieved by separating the state-object and the action + transitioner into separate components whereas the transitioners are considered to be request-handlers in a request-to-handler model. They respond to the input of an action and determine how a state transitions into other states. An action and the state-object itself are combined in a transaction which is the actual request model. There the actions are labels and models at the same time that hold the data for the requests.

### The problem

The transitioners hold the state-logic that is usually housed in the state-object itself, when regarding the standard State Pattern, but is confined to the amount of methods a state holds. The state in this case usually contains the same methods as the context it is considered to be the state for. So extending the amount of operations that require state specific transitions can only be achieved by modifying the class-structure of both the state and the context. 

![state](docs/standard-state.png)

Next to that the amount of responsibilities is increased, the abstraction for the those objects are growing as well and most configurations require the objects to communicate with one-another by the means of their concrete implementations instead of an abstraction. So altogether this pattern can cause some problems in regards to violating the SOLID design principles, although this is mostly accepted as this pattern is considered to be quite a well fitting solution. This library tries to solve the aforementioned issues and takes the inspiration from multiple other initiatives like, i.e., the Redux Pattern. 

### A solution

The library consists of state-machines that provide and create the state-objects for a specific context each. The state-objects refer to a provider/registry which stores all the different transitioner-objects from that state, where these transitioner-objects are stored in a key-value store by which the actions function as keys. When a state does contain a transitioner-object for a specific action, then that transitioner can be applied.

![state-machine](docs/St8-ment-state.png)

**Note:** The transitioner-objects are called transitioners because of there purpose/behavior, but be aware that a transitioner-object does not always handle a single state transition as it more or less an equivalent to the effects from the Redux libraries. They hold the state-logic like the methods in the standard state-pattern and can therefore make a decision to transition to one state or, when a specific condition fails for example, it could determine to transition to a different state or even none at all when no conditions are met and stay in their current state. But these choices lay in the hands of the developer of specific transitioners.

### How it works

The diagram shown below is there to support the understanding of the system and it bears quite some similarity to the Redux diagram that can be found quite easily online. The numbers in the diagram refer to the described steps. **Note:** A separate path that can be taken and is determined by condition is not shown as a number but will be described in the steps.

![st8-ment](docs/St8-ment-diagram.png)

**Steps:**

1. The context (mostly a system or aggregate-root) contains the state and an accept method that accepts an action. 
2. The actions is transported to the state and accepted by the state its own accept method.
3. The incoming action is being verified by the state, to determine whether there is a transitioner related to that action. This verification is achieved by determining whether the TransitionerProvider actually contains a transitioner for this action.
4. When this is the case, the chosen transitioner executes its logic by which it can then use the state-machine to choose the next state. 
5. The state is eventually set to the context and the cycle is complete. But when the transitioner cannot be found, a boolean determining the success of the state transition operation returned by the accepting methods to be false and no state changes have been made.