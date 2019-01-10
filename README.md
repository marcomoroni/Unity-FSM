# Finite state machine for Unity

This is one of the best finite state machines I found for Unity. The original code is from [Unity Gems](https://unitygem.wordpress.com/state-machine-fsm-part-2/), but I modified a little bit to include support for `FixedUpdate()` and `LateUpdate()` and I added a custom Inspector to see the current state.

## How to use

This FSM works as a component: `FiniteStateMachine` is an abstract class that inherit from `MonoBehaviour`, so to use it create a class that inherits from `FiniteStateMachine` and use it as a component.

``` cs
using UnityEngine;
using System;

public class Finn : FiniteStateMachine { }
```

Make sure you include `using System`.

Then add:
* an `enum` with the states
* the FSM iintialization with `InitializeFiniteStateMachine()`
* the legal transitions with `AddTransitionsToState()`
* the overridden `Update()`, `FixedUpdate()` and `LateUpdate()`

``` cs
public class Finn : FiniteStateMachine
{
	protected virtual void Awake()
	{
		// Initialize the FSM
		InitializeFiniteStateMachine<FinnStates>(FinnStates.Start);
		
		// Add legal transitions
		AddTransitionsToState(FinnStates.Start,
			new Enum[] { FinnStates.Walk });
		AddTransitionsToState(FinnStates.Walk,
			new Enum[] { FinnStates.Run });
		AddTransitionsToState(FinnStates.Run,
			new Enum[] { FinnStates.Walk });
	}
	
	protected override void Update()
	{
		base.Update();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
	
	protected override void LateUpdate()
	{
		base.LateUpdate();
	}
}

public enum FinnStates
{
	Start, Walk, Run
}
```

### State methods

For every state you can add an `Enter`, `Exit`, `Update`, `FixedUpdate` and `LateUpdate` function by simply adding the name of the state at the end. For example, for a state `Run` you can have

``` cs
void EnterRun(Enum previousState)
{
	// ...
}

void ExitRun(Enum nextState)
{
	// ...
}

void UpdateRun()
{
	// ...
}

void FixedUpdateRun()
{
	// ...
}

void LateUpdateRun()
{
	// ...
}
```

It work similarly to the `Update()` function in `Monobehavior`.

### Change state

To change state use

``` cs
ChangeCurrentState(FinnStates.Walk);
```

For example:

``` cs
protected override void Update()
{
	base.Update();
	if (Input.GetKeyDown(KeyCode.W)) { ChangeCurrentState(FinnStates.Walk); }
	if (Input.GetKeyDown(KeyCode.R)) { ChangeCurrentState(FinnStates.Run); }
}
```

### Debug mode

To initialize the FSM in debug mode add `true` as the second argument:

``` cs
InitializeFiniteStateMachine<FinnStates>(FinnStates.Start, true);
```

This lets you see more info in the console.
