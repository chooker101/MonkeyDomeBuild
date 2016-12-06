using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour
{
	public bool isGamePaused = false;
    public GameObject pauseUI;
    public GameObject volumeUI;
    public bool inOptions = false;


    // Update is called once per frame
    void Update()
    {
        if (CheckStartButton())
		{
            isGamePaused = !isGamePaused;

            if (isGamePaused)
			{
                inOptions = false;
                AudioEffectManager.Instance.PlayMenuButtonSE();
				Time.timeScale = 0;
                volumeUI.SetActive(false);
                pauseUI.SetActive(true);
                //call ui
            }
			else
			{
                AudioEffectManager.Instance.PlayUnMenuButtonSE();
                Time.timeScale = 1;
                pauseUI.SetActive(false);
                volumeUI.SetActive(false);
                //close ui
			}

		}
        if (isGamePaused)
        {
            if (CheckOtherButton())
            {
                inOptions = !inOptions;

                if (inOptions)
                {
                    volumeUI.SetActive(false);
                    pauseUI.SetActive(true);
                    //call ui
                }
                else
                {
                    volumeUI.SetActive(true);
                    pauseUI.SetActive(false);
                    //close ui
                }

            }
        }
	}

	bool CheckStartButton()
	{
		for(int i = 0;i < GameManager.Instance.TotalNumberofActors;++i)
		{
			if(GameManager.Instance.gmInputs[i].mStart)
			{
				return true;
			}
		}
		return false;
	}

    bool CheckOtherButton()
    {
        for (int i = 0; i < GameManager.Instance.TotalNumberofActors; ++i)
        {
            //other button for switching between
            if (GameManager.Instance.gmInputs[i].mJump)
            {
                return true;
            }
        }
        return false;
    }
}
