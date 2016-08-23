using UnityEngine;
using System.Collections;

public class AI : Actor
{
    enum State
    {
        Idle,
        Catch,
        Throw,
        Move
    }

    State currentState;

	// Use this for initialization
	void Start ()
	{
        currentState = State.Idle;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        ExecuteState();
	}

    void ExecuteState()
    {
        switch (currentState)
        {
            case State.Idle:
                currentState = ExecuteIdle();
                break;
            case State.Catch:
                currentState = ExecuteCatch();
                break;
            case State.Throw:
                currentState = ExecuteThrow();
                break;
            case State.Move:
                currentState = ExecuteMove();
                break;
            default :
                Debug.LogError("YOU FUCKING BROKE IT!");
                break;
        }

    }

    State ExecuteIdle()
    {
        if(GameManager.Instance.gmUIManager.matchTime < GameManager.Instance.gmUIManager.startMatchTime)
        {
            return State.Move;
        }

        return State.Idle;
    }

    State ExecuteCatch()
    {
        //TODO Catch Logic
        return currentState;
    }

    State ExecuteThrow()
    {
        //TODO Target Selection and Throw Logic
        return currentState;
    }

    State ExecuteMove()
    {
        if(characterType is Monkey) //TODO Monkey Move Logic
        {

        }
        else //TODO Gorilla Move Logic
        {

        }

        return currentState;
    }
}
