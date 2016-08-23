using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColourChanger : MonoBehaviour
{

    public int playerTargetNumber = -1;
    
    private RecordKeeper recordKeeper;
    private CircleCollider2D targetCollider;
    private GameObject[] players;
    private GameObject myPlayer = null;
    private GameObject objectHit = null;
    private Material materialToApply;

    // Use this for initialization
    void Start()
    {
        // Find Sphere collider for target within children
        targetCollider = GetComponent<CircleCollider2D>();
        recordKeeper = FindObjectOfType<RecordKeeper>();

        // Fills a list with all current players
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<Player>().whichplayer == playerTargetNumber) // Finds the player object that this target correponds to
            {
                myPlayer = players[i];
                return;
            }
            else
            {
                myPlayer = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Ball") )
        {
            objectHit = other.GetComponentInParent<BallInfo>().gameObject;
            Debug.Log("Colour Target: Ball found");
            if(objectHit != null)
            {
                materialToApply = objectHit.GetComponent<BallInfo>().mySpriteColour; // Get the material from the ball

                if (objectHit.GetComponent<BallInfo>().playerThrewLast == playerTargetNumber) // If the player who threw the ball is the one for this target
                {
                    Debug.Log("Colour Target: This is my player");

                    for (int i = 0; i < recordKeeper.GetComponent<RecordKeeper>().colourPlayers.Length; i++)
                    {
                        Debug.Log("Colour Target: Looking for player: " + i.ToString());

                        if (i == myPlayer.GetComponent<Player>().whichplayer)
                        {
                            recordKeeper.GetComponent<RecordKeeper>().colourPlayers[i] = materialToApply;

                            Debug.Log("Colour logged into RecordKeeper" + i.ToString());
                            break;
                        }
                    }
                }
                else
                {
                    Debug.Log("Colour Target: Not my player!");
                }
            }
        }
    }
}
