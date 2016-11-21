using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : ScriptableObject
{
	public InputManager(Vector2 ixy,bool icatch,bool ijump,bool iaimstomp)
	{
		mXY = ixy;
		mCatch = icatch;
		mJump = ijump;
		mAimStomp = iaimstomp;
	}
	public Vector2 mXY;
	public bool mCatch;
	public bool mJump;
	public bool mAimStomp;
    public bool mChargeStomp;
    public bool mChargeThrow;
    public bool mCatchRelease;
	public bool mStart;

    public void Reset()
    {
        mXY = Vector2.zero;
        mCatch = false;
        mJump = false;
        mAimStomp = false;
        mChargeStomp = false;
        mChargeThrow = false;
        mCatchRelease = false;
        mStart = false;
    }
}
