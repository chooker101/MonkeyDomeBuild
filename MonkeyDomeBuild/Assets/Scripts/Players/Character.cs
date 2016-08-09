using UnityEngine;
using System.Collections;

public class Character
{
	public Vector3 gorillaSize = GameManager.Instance.gmMovementManager.gScale;
	public Vector3 monkeySize = GameManager.Instance.gmMovementManager.mScale;
	public float throwForce;
	public float jumpforce;
	public float movespeed;


	public virtual void CHUpdate()
	{

	}

	public virtual void CHFixedUpdate()
	{

	}

	public virtual float GetTimeBeingGorilla() { return 0.0f; }

	public virtual void Mutate() { }

}
