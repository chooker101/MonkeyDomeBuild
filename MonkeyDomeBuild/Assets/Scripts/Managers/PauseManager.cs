using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour
{
	public bool isGamePaused = false;
	
	// Update is called once per frame
	void Update ()
	{
		if (((GameManager.Instance.gmInputs[0].mStart || GameManager.Instance.gmInputs[0].mStart) || (GameManager.Instance.gmInputs[0].mStart || GameManager.Instance.gmInputs[0].mStart)) || GameManager.Instance.gmInputs[0].mStart)
		{
			if (isGamePaused)
			{
				Time.timeScale = 0;
				//call ui
			}
			else
			{
				Time.timeScale = 1;
			}
		}
	}
}
