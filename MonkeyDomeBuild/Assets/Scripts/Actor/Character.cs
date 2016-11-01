using UnityEngine;
using System.Collections;

public class Character : ScriptableObject
{
	public Vector3 gorillaSize = GameManager.Instance.gmMovementManager.gScale;
	public Vector3 monkeySize = GameManager.Instance.gmMovementManager.mScale;
	public bool isCharging = false;
	public float throwForce;
	public float jumpforce;
	public float movespeed;
	public float chargespeed;
	protected int myPlayer;
	protected Actor cacheplayer;


	public virtual void CHUpdate()
	{

	}

	public virtual void CHFixedUpdate()
	{

	}

	public virtual float GetTimeBeingGorilla() { return 0.0f; }

	public virtual void Mutate() { }

}
