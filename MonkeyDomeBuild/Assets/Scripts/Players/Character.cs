using UnityEngine;
using System.Collections;

public class Character
{
	public float scaleSize = 2;
	public float moveForce;
	public float speedLimit;
	public float jumpForce;
	public float throwForce;
	public float downForce;
	public float downForceIncrement;
	public float tempDownForce;
	public float climbSpeedLimit;
	public float climbDrag;
	public float normalDrag;
	public float climbForce;
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
