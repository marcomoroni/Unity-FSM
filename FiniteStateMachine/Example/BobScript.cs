using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BobScript : FiniteStateMachine
{
	private float timer = 0f;
	private Enum previousState;

	protected virtual void Awake()
	{
		InitializeFiniteStateMachine<BobStates>(BobStates.Start);
		AddTransitionsToState(BobStates.Start,
			new Enum[] { BobStates.Walk });
		AddTransitionsToState(BobStates.Walk,
			new Enum[] { BobStates.Run, BobStates.Attack, BobStates.Pause, BobStates.Die });
		AddTransitionsToState(BobStates.Run,
			new Enum[] { BobStates.Walk, BobStates.Attack, BobStates.Pause, BobStates.Die });
		AddTransitionsToState(BobStates.Attack,
			new Enum[] { BobStates.Walk, BobStates.Run, BobStates.Pause, BobStates.Die });
		AddTransitionsToState(BobStates.Pause,
			new Enum[] { BobStates.Walk, BobStates.Attack, BobStates.Run });
		AddTransitionsToState(BobStates.Die,
			new Enum[] { BobStates.Start });
	}

	void EnterStart(Enum previous)
	{
		if((BobStates)previous == BobStates.Die)
		{
			Debug.Log("I am back from the dead");
		}
		Debug.Log("Starting Bob");
	}

	void EnterWalk(Enum previous) { Debug.Log("Setting walk"); }
	void UpdateWalk() { Debug.Log("I am walking"); }
	void FixedUpdateWalk() { Debug.Log("Physics calculations also"); }

	void EnterRun(Enum previous) { Debug.Log("EnterRun"); }
	void UpdateRun() { Debug.Log("I am running"); }

	void EnterAttack(Enum previous)
	{
		timer = 0f;
		this.previousState = previous;
	}
	void UpdateAttack()
	{
		Debug.Log("I am attacking");
		timer += Time.deltaTime;
		if (timer >= 1.0f) { ChangeCurrentState(previousState); }
	}

	void EnterPause(Enum previous)
	{
		previousState = previous;
		// Stopping the game!!
	}
	void UpdatePause()
	{
		if (Input.GetKeyDown(KeyCode.P) && (BobStates)CurrentState != BobStates.Pause) { ChangeCurrentState(previousState); }
	}
	void ExitPause(Enum nextState)
	{
		// Restarting the game!!
	}

	void EnterDie(Enum next) { Debug.Log("I am so dead right now"); }
	void UpdateDie()
	{
		// Should we restart
		if (Input.GetKeyDown(KeyCode.S)) { ChangeCurrentState(BobStates.Start); }
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.P) && (BobStates)CurrentState != BobStates.Pause) { ChangeCurrentState(BobStates.Pause); }
		if (Input.GetKeyDown(KeyCode.W)) { ChangeCurrentState(BobStates.Walk); }
		if (Input.GetKeyDown(KeyCode.A)) { ChangeCurrentState(BobStates.Attack); }
		if (Input.GetKeyDown(KeyCode.R)) { ChangeCurrentState(BobStates.Run); }
		if (Input.GetKeyDown(KeyCode.D)) { ChangeCurrentState(BobStates.Die); }
	}
}

public enum BobStates
{
	Start, Walk, Run, Attack, Die, Pause
}