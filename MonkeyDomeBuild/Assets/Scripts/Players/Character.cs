using UnityEngine;
using System.Collections;

public class Character
{
	public Vector3 gorillaSize = new Vector3(2.0f, 3.0f, 1.0f);
	public Vector3 monkeySize = new Vector3(1.5f, 2.0f, 1.0f);
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
