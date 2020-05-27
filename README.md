# St8-ment
A dynamic state/state-machine pattern library for SOLID state pattern design. This is achieved by separating the state-object and the action + transitions into separate components whereas the transitions are considered to be request-handlers in a request-to-handler model. They respond to the input of an action and determine how a state transitions into another state. An action and the state-object itself are combined in a transaction which is the actual request model. There the actions are labels and models at the same time that hold the data for the requests.

### The problem

The transitions hold the state-logic that is usually housed in the state-object itself, when regarding the standard State Pattern, but is confined to the amount of methods a state holds. The state in this case usually contains the same methods as the context it is considered to be the state for. So extending the amount of operations that require state specific transitions can only be achieved by modifying the class-structure of both the state and the context. 

![state](docs\standard-state.png)

Next to that the amount of responsibilities is increased, the abstraction for the those objects are growing as well and the most configurations require the objects to communicate with one-another by the means of their concrete implementations instead of an abstraction. So altogether this pattern can cause some problems in regards to violating the SOLID design principles, although this is mostly accepted as this pattern is considered to be quite a well fitting solution. This library tries to solve the aforementioned issues and takes the inspiration from multiple other initiatives like, i.e., the Redux Pattern. 

### A solution

The library consists of state-machines that provide and create the state-objects for a specific context each. The state-objects refer to a provider/registry which stores all the different transition-objects from that state, where these transition-objects are stored in a key-value store by which the actions function as keys. When a state does contain a transition-object for a specific action, then that transition can be applied.

![state-machine](docs\St8-ment-state.png)

**Note:** The transition-objects are called transitions because of there purpose/behavior, but be aware that a transition-object is not always a single transition as it more or less an equivalent to the effects from the Redux libraries. They hold the state-logic like the methods in the standard state-pattern and can therefore make a decision to transition to one state or, when a specific condition fails for example, it could determine to transition to a different state or even none at all when no conditions are met. But these choices lay in the hands of the developer of specific transitions.

### How it works

![](docs\St8-ment-diagram.png)