using UnityEngine;
using System.Collections;

public class FSM<T>
{
	private T owner;
	private State<T> currentState;
	private State<T> previousState;

	public FSM( T owner, State<T> initial )
	{
		this.owner = owner;
		currentState = initial;
	}

	public void Update()
	{
		currentState.Action ( owner );
	}

	public void ChangeState( State<T> NewState )
	{
		//Save current state to previous state
		previousState = currentState;

		//Perform exit action
		currentState.BeforeExit (owner);

		//Assigning new state and executing the pre-enter action
		currentState = NewState;
		currentState.BeforeEnter ( owner );
	}

	public State<T> GetCurrentState()
	{
		return currentState;
	}
}