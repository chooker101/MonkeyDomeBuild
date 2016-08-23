using UnityEngine;
using System.Collections;

public class RecordKeeper : MonoBehaviour
{
    public Material[] colourPlayers;
    public int[] scoreEndPlayers;
    public int playerGorilla = -1;

    public Material defaultColour;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        
        for(int i = 0; i<colourPlayers.Length; i++)
        {
            colourPlayers[i] = defaultColour;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}