using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour
{
	public bool isGamePaused = false;
    public GameObject pauseUI;

    // Update is called once per frame
    void Update()
    {
        if (((GameManager.Instance.gmInputs[0].mStart || GameManager.Instance.gmInputs[1].mStart) || (GameManager.Instance.gmInputs[2].mStart || GameManager.Instance.gmInputs[3].mStart)) || GameManager.Instance.gmInputs[4].mStart)
		{
            isGamePaused = !isGamePaused;

            if (isGamePaused)
			{
				Time.timeScale = 0;
                pauseUI.SetActive(true);
                //call ui
            }
			else
			{
				Time.timeScale = 1;
                pauseUI.SetActive(false);
                //close ui
			}
		}
	}
}
