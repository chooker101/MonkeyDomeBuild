using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelObjectsScript : MonoBehaviour
{
	public List<GameObject> loPlatforms;
	public List<GameObject> loVines;
	public GameObject stage;
	public GameObject stageVines;
	public int numberOfLevels = 10;

	void Awake()
	{
		LevelObject[] platforms = stage.GetComponentsInChildren<LevelObject>();
		foreach(LevelObject T in platforms)
		{
			loPlatforms.Add(T.gameObject);
		}
		LevelObject[] vines = stageVines.GetComponentsInChildren<LevelObject>();
		foreach (LevelObject T in vines)
		{
			loVines.Add(T.gameObject);
		}
		GameManager.Instance.gmLevelObjectsScript = GetComponent<LevelObjectsScript>();
	}
}
