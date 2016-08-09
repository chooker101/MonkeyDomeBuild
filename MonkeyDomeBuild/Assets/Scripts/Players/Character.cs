using UnityEngine;
using System.Collections;

public class Character
{
	public Vector3 gorillaSize = new Vector3(2.0f, 2f, 2f);
	public Vector3 monkeySize = new Vector3(1f, 1f, 1.0f);
	public float horizontalMoveForce;
	public float speedLimit;
	public float jumpForce;
	public float throwForce;
	public float downForce;
	public float downForceIncrement;
	public float tempDownForce;
	public float climbSpeedLimit;
	public float climbingHorizontalMoveSpeed;
    public float climbingVerticalMoveSpeed;
	public float maxDownForce;

	public virtual void CHUpdate()
	{

	}

	public virtual void CHFixedUpdate()
	{

	}

	public virtual float GetTimeBeingGorilla() { return 0.0f; }

	public virtual void Mutate() { }

}
