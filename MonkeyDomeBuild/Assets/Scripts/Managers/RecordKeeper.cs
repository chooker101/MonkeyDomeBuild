using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RecordKeeper : MonoBehaviour
{
    public List<Material> colourPlayers = new List<Material>();
    public List<Image> playerLocators = new List<Image>();
    public int[] scoreEndPlayers;
    public int playerGorilla = -1;
    public Material defaultColour;

    private bool gorillaSmashed = false;

    // Use this for initialization
    void Start()
    {
        //DontDestroyOnLoad(this);
        
        for(int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            colourPlayers.Add(defaultColour);
        }
    }
    public void SetPlayerMaterial(int playerIndex, Material mat)
    {
        if (playerIndex + 1 > colourPlayers.Count)
        {
            InitNewPlayerColour(playerIndex);
        }
        colourPlayers[playerIndex] = mat;
    }
    void InitNewPlayerColour(int playerIndex)
    {
        colourPlayers.Add(defaultColour);
        if (playerIndex > colourPlayers.Count)
        {
            InitNewPlayerColour(playerIndex);
        }
    }
    public void ResetPlayerMaterial(int playerIndex)
    {
        if (playerIndex < colourPlayers.Count)
        {
            colourPlayers[playerIndex] = defaultColour;
        }
    }
    public Color GetPlayerColour(int playerIndex)
    {
        if (playerIndex + 1 > colourPlayers.Count)
        {
            InitNewPlayerColour(playerIndex);
        }
        return colourPlayers[playerIndex].color;
    }
    public Color GetPlayerLocator(int playerIndex)
    {
        if (playerIndex + 1 > colourPlayers.Count)
        {
            InitNewPlayerColour(playerIndex);
        }
        return colourPlayers[playerIndex].color;
    }
}