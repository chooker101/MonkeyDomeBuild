using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMatchCountDown : MonoBehaviour
{
    public Text countText;
    private float timeCount;
    private float perTime = 1f;
    public void StartCountDown()
    {
        StartCoroutine(CountDown());
    }
    IEnumerator CountDown()
    {
        Vector3 startLoc = new Vector3(0, 0, 0);
        Vector3 startScale = new Vector3(1.2f, 1.2f, 1.2f);
        Vector3 finalScale = new Vector3(1f, 1f, 1f);

        for(int i = 3; i >= 1; i--)
        {
            countText.text = i.ToString();
            countText.transform.localPosition = startLoc;
            //countText.transform.localScale = startScale;
            countText.gameObject.SetActive(true);
            timeCount = perTime;
            while (timeCount > 0)
            {
                //countText.transform.localPosition = Vector3.LerpUnclamped(startLoc, Vector3.zero, (perTime - timeCount) / perTime);
                //countText.transform.localScale = Vector3.LerpUnclamped(startScale, finalScale, (perTime - timeCount) / perTime);
                timeCount -= Time.deltaTime;
                yield return null;
            }
        }
        yield return null;
        countText.text = "Start";
        countText.transform.localScale = finalScale;
        yield return new WaitForSeconds(1f);
        countText.gameObject.SetActive(false);
        GameManager.Instance.playerCanMove = true;
    }

}
