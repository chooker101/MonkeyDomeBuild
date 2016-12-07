using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;

public class BallColorManager : MonoBehaviour
{
    public List<BallColor> playerColors = new List<BallColor>();
    public AnimatorController whiteAnim;
    public AnimatorController yellowAnim;
    public AnimatorController greenAnim;
    public AnimatorController redAnim;
    public AnimatorController purpleAnim;
    public AnimatorController brownAnim;

    void Start()
    {
        GameManager.Instance.gmBallColorManager = this;
    }
    public void ChangePlayerColor(int playerIndex, BallColor color)
    {
        if (playerColors[playerIndex] != color)
        {
            playerColors[playerIndex] = color;
        }
        AnimatorController newAnim;
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
