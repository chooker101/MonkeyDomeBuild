using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GamplaySettings : MonoBehaviour
{
	public Text botsText;
	public Text gamemodeText;
	public Text mapText;

	bool dpadXOnCooldown = false;
	bool dpadYOnCooldown = false;

	int currEditable = 0;
	int prevEditable = 0;
	

	void Update()
	{
		//Debug.Log(nextGameMode.ToString());
		if(GameManager.Instance.allDpad.x != 0.0f && !dpadXOnCooldown)
		{
			dpadXOnCooldown = true;
			StartCoroutine(DpadXCooldown());
			if (GameManager.Instance.allDpad.x > 0.0f)
			{
				prevEditable = currEditable;
				currEditable = (currEditable + 1) % 3;
			}
			else
			{
				prevEditable = currEditable;
				currEditable--;
				currEditable = (currEditable < 0) ? 2 : currEditable;
			}
		}

		switch (currEditable)
		{
			case 0:
				{
					if (GameManager.Instance.allDpad.y != 0.0f && !dpadYOnCooldown)
					{
						dpadYOnCooldown = true;
						StartCoroutine(DpadYCooldown());
						if (GameManager.Instance.allDpad.y > 0.0f)
						{
							GameManager.Instance.numOfBotsUI = (GameManager.Instance.numOfBotsUI + 1) % (6 - (int)GameManager.Instance.TotalNumberofPlayers);
						}
						else
						{
							GameManager.Instance.numOfBotsUI--;
							GameManager.Instance.numOfBotsUI = (GameManager.Instance.numOfBotsUI < 0) ? (5 - (int)GameManager.Instance.TotalNumberofPlayers) : GameManager.Instance.numOfBotsUI;
						}
					}
					botsText.text = ("<Bots: " + GameManager.Instance.numOfBotsUI + ">");
					if(prevEditable == 1)
					{
						gamemodeText.text = ("Gamemode: " + GameManager.Instance.nextGameModeUI.ToString());
					}
					else if(prevEditable == 2)
					{
						mapText.text = ("Map: " + GameManager.Instance.nextSceneUI.ToString());
					}
					break;
				}
			case 1:
				{
					if (GameManager.Instance.allDpad.y != 0.0f && !dpadYOnCooldown)
					{
						dpadYOnCooldown = true;
						StartCoroutine(DpadYCooldown());
						if (GameManager.Instance.allDpad.y > 0.0f)
						{
							GameManager.Instance.nextGameModeUI = (GameManager.GameMode)(((int)GameManager.Instance.nextGameModeUI + 1) % (int)GameManager.GameMode.Length);
						}
						else
						{
							GameManager.Instance.nextGameModeUI--;
							GameManager.Instance.nextGameModeUI = (GameManager.GameMode)(((int)GameManager.Instance.nextGameModeUI < 0) ? ((int)GameManager.GameMode.Length - 1) : (int)GameManager.Instance.nextGameModeUI);
						}
					}
					gamemodeText.text = ("<Gamemode: " + GameManager.Instance.nextGameModeUI.ToString() + ">");
					if (prevEditable == 0)
					{
						botsText.text = ("Bots: " + GameManager.Instance.numOfBotsUI);
					}
					else if (prevEditable == 2)
					{
						mapText.text = ("Map: " + GameManager.Instance.nextSceneUI.ToString());
					}
					break;
				}
			case 2:
				{
					if (GameManager.Instance.allDpad.y != 0.0f && !dpadYOnCooldown)
					{
						dpadYOnCooldown = true;
						StartCoroutine(DpadYCooldown());
						if (GameManager.Instance.allDpad.y > 0.0f)
						{
							GameManager.Instance.nextSceneUI = (GameManager.Scene)(((int)GameManager.Instance.nextSceneUI + 1) % (int)GameManager.Scene.Length);
						}
						else
						{
							GameManager.Instance.nextSceneUI--;
							GameManager.Instance.nextSceneUI = (GameManager.Scene)(((int)GameManager.Instance.nextSceneUI < 0) ? ((int)GameManager.Scene.Length - 1) : (int)GameManager.Instance.nextSceneUI);
						}
					}
					mapText.text = ("<Map: " + GameManager.Instance.nextSceneUI.ToString() + ">");
					if (prevEditable == 0)
					{
						botsText.text = ("Bots: " + GameManager.Instance.numOfBotsUI);
					}
					else if (prevEditable == 1)
					{
						gamemodeText.text = ("Gamemode: " + GameManager.Instance.nextGameModeUI.ToString());
					}
					break;
				}
			default:
				break;
		}
	}

	IEnumerator DpadXCooldown()
	{
		yield return new WaitForSeconds(0.15f);
		dpadXOnCooldown = false;
	}
	IEnumerator DpadYCooldown()
	{
		yield return new WaitForSeconds(0.15f);
		dpadYOnCooldown = false;
	}
}
