using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallColorManager : MonoBehaviour
{
    public List<BallColor> playerColors = new List<BallColor>();
    public RuntimeAnimatorController whiteAnim;
    public RuntimeAnimatorController yellowAnim;
    public RuntimeAnimatorController greenAnim;
    public RuntimeAnimatorController redAnim;
    public RuntimeAnimatorController purpleAnim;
    public RuntimeAnimatorController brownAnim;

    void Start()
    {
        GameManager.Instance.gmBallColorManager = this;
        for(int i = 0; i < 5; i++)
        {
            playerColors.Add(BallColor.White);
        }
    }
    public void ChangePlayerColor(int playerIndex, BallColor color)
    {
        if (playerColors[playerIndex] != color)
        {
            playerColors[playerIndex] = color;
        }
        RuntimeAnimatorController newAnim;
        switch (color)
        {
            default:
                goto case BallColor.White;
            case BallColor.White:
                newAnim = whiteAnim;
                break;
            case BallColor.Yellow:
                newAnim = yellowAnim;
                break;
            case BallColor.Green:
                newAnim = greenAnim;
                break;
            case BallColor.Red:
                newAnim = redAnim;
                break;
            case BallColor.Purple:
                newAnim = purpleAnim;
                break;
            case BallColor.Brown:
                newAnim = brownAnim;
                break;
        }
        GameManager.Instance.gmPlayerScripts[playerIndex].GetComponentInChildren<Animator>().runtimeAnimatorController = newAnim;
    }

}
