using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColourChanger : MonoBehaviour {

    public int playerTargetNumber = -1;

    private CircleCollider2D targetCollider;
    private GameObject[] players;
    private GameObject myPlayer;
    private Material materialToApply;
    
    // Use this for initialization
    void Start () {

        // Find Sphere collider for target within children
        targetCollider = GetComponent<CircleCollider2D>();

        // Fills a list with all current players
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].GetComponent<Player>().whichplayer == playerTargetNumber) // Finds the player object that this target correponds to
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
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colour Target Hit");
        GameObject objectHit = other.gameObject;

        if(objectHit.gameObject.tag == "Ball")
        {
            Debug.Log("Colour Target: Ball found");

            materialToApply = objectHit.gameObject.GetComponent<BallInfo>().mySpriteColour; // Get the material from the ball

            if(objectHit.GetComponent<BallInfo>().playerThrewLast == playerTargetNumber) // If the player who threw the ball is the one for this target
            {
                Debug.Log("Colour Target: Material Applied");
                myPlayer.GetComponent<SpriteRenderer>().material = materialToApply; // Apply the material from the ball
            }
            else
            {
                Debug.Log("Colour Target: Not my player!");
            }
        }
    }
}
