using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LifeImgHolder : MonoBehaviour
{
    public List<Image> lifeImgs = new List<Image>();
    public void Init()
    {
        foreach(Image i in lifeImgs)
        {
            i.gameObject.SetActive(true);
        }
    }
    public void LifeDown()
    {
        for(int i = lifeImgs.Count - 1; i >= 0; i--)
        {
            if (lifeImgs[i].isActiveAndEnabled)
            {
                lifeImgs[i].gameObject.SetActive(false);
                break;
            }
        }
    }
}
