using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelObjectScript : MonoBehaviour
{
	public List<GameObject> loPlatforms;
	public List<GameObject> loVines;

	void Awake()
	{
		GameManager.Instance.gmLevelObjectScript = GetComponent<LevelObjectScript>();
	}
}
