# Tween System for Unity
 
Tween System is a very simple fluent sequence tweener for UI elements.

- tween anchored position and alpha / transparency
- three movement sets for each element: *Set*, *Show*, *Hide*
- fluent sequence creation (e.g. *Show, wait 1s, then hide*)
- callbacks between states

### Usage

Each movement set (*Set*, *Show*, *Hide*) must have a specified entry state (position and alpha), an exit state, and duration.
Several examples of usage:

```csharp
private Tweener tweener;

// *Begin* begins a new sequence
// *Set* is meant to instantaneously tween to a "safe" state (eg outside the screen, invisible)
tweener.Begin().Set();

// *Show* and *Hide* are meant to move the element in and out of a desired position
tweener.Begin().Show();
tweener.Begin().Hide();

// Here the element will begin *Hide* immediately after completing *Show*
tweener.Begin().Show().Hide();

// Hide and then move to a "safe" state
tweener.Begin().Hide().Set();

// You can specify a duration in seconds to wait between tweens
tweener.Begin().Show().Wait(2).Hide().Set();

// Make a callback after completing Show
tweener.Begin().Show().Call(() => Test(this));
```