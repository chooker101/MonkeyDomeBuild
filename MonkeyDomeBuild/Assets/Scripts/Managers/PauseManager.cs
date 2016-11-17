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
        if (CheckStartButton())
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

	bool CheckStartButton()
	{
		for(int i = 0;i < GameManager.Instance.TotalNumberofPlayers;++i)
		{
			if(GameManager.Instance.gmInputs[i].mStart)
			{
				return true;
			}
		}
		return false;
	}
}
