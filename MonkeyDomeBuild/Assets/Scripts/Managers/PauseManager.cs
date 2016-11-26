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
                AudioEffectManager.Instance.PlayMenuButtonSE();
				Time.timeScale = 0;
                pauseUI.SetActive(true);
                //call ui
            }
			else
			{
                AudioEffectManager.Instance.PlayUnMenuButtonSE();
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
