using UnityEngine;
using System.Collections;


public class VictoryRoomManager : MonoBehaviour {
	public float speed;
	public float heightDeviser;

	public GameObject Podium1;
	public GameObject Podium2;
	public GameObject Podium3;
	public GameObject Podium4;
	public GameObject Podium5;
	private float p1score;
	private float p2score;
	private float p3score;
	private float p4score;
	private float p5score;
	public ScoringManager scoreKeeper;


	// Use this for initialization
	void Start () {
		//scoreKeeper = GameObject.Find ("GameManager").GetComponent<ScoringManager>();
		p1score = scoreKeeper.p1Score/heightDeviser;
		p2score = scoreKeeper.p2Score/heightDeviser;
		p3score = scoreKeeper.p3Score/heightDeviser;
		p4score = scoreKeeper.p4Score/heightDeviser;
		p5score = scoreKeeper.p5Score/heightDeviser;

		/*
		Podium1 = GameObject.Find ("Podium1");
		Podium2 = GameObject.Find ("Podium2");
		Podium3 = GameObject.Find ("Podium3");
		Podium4 = GameObject.Find ("Podium4");
		Podium5 = GameObject.Find ("Podium5");
		*/

	}
	
	// Update is called once per frame
	void Update () {
		Podium1.transform.position = Vector3.MoveTowards (Podium1.transform.position ,new Vector3 (Podium1.transform.position.x, p1score, Podium1.transform.position.z),speed*Time.deltaTime);
		Podium2.transform.position = Vector3.MoveTowards (Podium2.transform.position ,new Vector3 (Podium2.transform.position.x, p2score, Podium2.transform.position.z),speed*Time.deltaTime);
		Podium3.transform.position = Vector3.MoveTowards (Podium3.transform.position ,new Vector3 (Podium3.transform.position.x, p3score, Podium3.transform.position.z),speed*Time.deltaTime);
		Podium4.transform.position = Vector3.MoveTowards (Podium4.transform.position ,new Vector3 (Podium4.transform.position.x, p4score, Podium4.transform.position.z),speed*Time.deltaTime);
		Podium5.transform.position = Vector3.MoveTowards (Podium5.transform.position ,new Vector3 (Podium5.transform.position.x, p5score, Podium5.transform.position.z),speed*Time.deltaTime);

	}
}
