using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Actor
{
    public override void CheckInputs()
    {
		virtualinput = GameManager.Instance.gmInputs[whichplayer].mXY;
		/*
        switch (whichPlayer)
        {
            case 1:
                mX = Input.GetAxis("p1_joy_x");
                mY = -Input.GetAxis("p1_joy_y");
                mCatch = Input.GetButtonDown("p1_catch/throw");
                mJump = Input.GetButtonDown("p1_jump");
                if(GetComponent<Player>() is GorillaAction)
                {
                    mAimStomp = Input.GetButtonDown("p1_aim/stomp");
                }
                else
                {
                    mAimStomp = Input.GetButton("p1_aim/stomp");
                }
                //mClimb = Input.GetButtonDown("p1_climb");
                //temp keyboard input
                if (Input.GetKey(KeyCode.A))
                {
                    mX = -1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    mX = 1;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    mY = 1;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    mY = -1;
                }
                break;

            case 2:
                mX = Input.GetAxis("p2_joy_x");
                mY = -Input.GetAxis("p2_joy_y");
                mJump = Input.GetButtonDown("p2_jump");
                mCatch = Input.GetButtonDown("p2_catch/throw");
                if (GetComponent<Player>() is GorillaAction)
                {
                    mAimStomp = Input.GetButtonDown("p2_aim/stomp");
                }
                else
                {
                    mAimStomp = Input.GetButton("p2_aim/stomp");
                }
                //mClimb = Input.GetButtonDown("p2_climb");
                break;

            case 3:
                mX = Input.GetAxis("p3_joy_x");
                mY = -Input.GetAxis("p3_joy_y");
                mJump = Input.GetButtonDown("p3_jump");
                mCatch = Input.GetButtonDown("p3_catch/throw");
                if (GetComponent<Player>() is GorillaAction)
                {
                    mAimStomp = Input.GetButtonDown("p3_aim/stomp");
                }
                else
                {
                    mAimStomp = Input.GetButton("p3_aim/stomp");
                }
                //mClimb = Input.GetButtonDown("p3_climb");
                break;
        }
		*/
		/*
        if (GameManager.Instance.gmInputs[whichplayer].mJump)
        {
            if (isClimbing)
            {
                if (GameManager.Instance.gmInputs[whichplayer].mXY.y < 0)
                {
					GameManager.Instance.gmInputs[whichplayer].mJump = false;
                }
                isClimbing = false;
                
            }
            else if (canClimb && !isClimbing)
            {
                //if (!GetComponent<Rigidbody2D>().isKinematic) ChangeIsKinematic();
                isClimbing = true;
                canJump = true;
				GameManager.Instance.gmInputs[whichplayer].mJump = false;
            }
        }*/
		/*1
        if (isClimbing)
        {
            if (GetComponent<Rigidbody>().drag != characterType.climbDrag)
            {
				GetComponent<Rigidbody>().drag = characterType.climbDrag;
            }
        }
        else
        {
            if (GetComponent<Rigidbody>().drag != characterType.normalDrag)
            {
				GetComponent<Rigidbody>().drag = characterType.normalDrag;
				characterType.tempDownForce = characterType.downForce;
            }
        }*/
	}

}

