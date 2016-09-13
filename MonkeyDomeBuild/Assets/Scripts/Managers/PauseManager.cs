using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{
	public bool isGamePaused = false;
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButton("space"))
		{
			isGamePaused = true;
			Time.timeScale = 0;
		}
		while (isGamePaused)
		{
			if(Input.GetButton("space"))
			{
				isGamePaused = false;
				Time.timeScale = 1;
			}
		}
	}
}
