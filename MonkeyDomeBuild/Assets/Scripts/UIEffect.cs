using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour {

    private Text myText;
    private bool scalingUp;
    public int aliveCounter;

    private Vector3 tempScale;
    private Vector3 originalScale;
    // Use this for initialization
    void Start()
    {
        scalingUp = true;
        myText = GetComponent<Text>();
        originalScale = myText.transform.localScale;

    }

    // Update is called once per frame
    void Update () {
        if (scalingUp)
        {
            ScaleUpText();
        }
        else
        {
            ScaleDownText();
        }

	}

     void ScaleUpText()
    {
        tempScale = myText.transform.localScale;

       tempScale.x = Mathf.Lerp(myText.transform.localScale.x, 2f, 1f*Time.deltaTime);
       tempScale.y = Mathf.Lerp(myText.transform.localScale.y, 2f, 1f*Time.deltaTime);

        myText.transform.localScale = tempScale;
        StartCoroutine(ScalingTime());
    }

     void ScaleDownText()
    {
        tempScale = myText.transform.localScale;

        tempScale.x = Mathf.Lerp(myText.transform.localScale.x, originalScale.x, 5f * Time.deltaTime);
        tempScale.y = Mathf.Lerp(myText.transform.localScale.y, originalScale.y, 5f * Time.deltaTime);

        myText.transform.localScale = tempScale;
        scalingUp = true;
    }

     IEnumerator ScalingTime()
    {
        yield return new WaitForSeconds(.5f);
        scalingUp = false;
    }
}
