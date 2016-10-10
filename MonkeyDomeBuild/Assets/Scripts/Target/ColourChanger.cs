using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ColourChanger : MonoBehaviour
{

    public int playerTargetNumber = -1;
    private int playerTargetNumberRegular = 0;
    public Text targetText;

    //private RecordKeeper recordKeeper;
    //private CircleCollider2D targetCollider;
    
    private GameObject myPlayer = null;
    private GameObject objectHit = null;
    private Material materialToApply;
    public bool isHit = false;

    // Use this for initialization
    void Start()
    {
		// Fills a list with all current players
		myPlayer = GameManager.Instance.gmPlayers[playerTargetNumber];
        playerTargetNumberRegular = playerTargetNumber + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHit)
        {
            targetText.text = "P" + playerTargetNumberRegular.ToString() + " READY!";
        }
        else
        {
            targetText.text = "P" + playerTargetNumberRegular.ToString() + " NOT READY";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Ball") )
        {
            objectHit = other.GetComponentInParent<BallInfo>().gameObject;
            //Debug.Log("Colour Target: Ball found");
            if(objectHit != null)
            {
                materialToApply = objectHit.GetComponent<BallInfo>().mySpriteColour; // Get the material from the ball

                if (objectHit.GetComponent<BallInfo>().playerThrewLast == playerTargetNumber) // If the player who threw the ball is the one for this target
                {
                    //Debug.Log("Colour Target: This is my player");
                    isHit = true;

                    for (int i = 0; i < GameManager.Instance.gmRecordKeeper.colourPlayers.Length; i++)
                    {
                        if (i == myPlayer.GetComponent<Player>().playerIndex)
                        {
							GameManager.Instance.gmRecordKeeper.colourPlayers[i] = materialToApply;

                           //Debug.Log("Colour logged into RecordKeeper: " + i.ToString());
                            break;
                        }
                    }
                }
                else
                {
                    //Debug.Log("Colour Target: Not my player!");
                }
            }
        }
    }
}
