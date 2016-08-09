using UnityEngine;
using System.Collections;

public class Gorillapickertest : MonoBehaviour {

    public int numPlayers;
    public int offsetAngleBy;
    public float BaserotationSpeed;
    public float SlowdownSpeed = 3;
    public int randomint;
    public float stopAngle;
    public GameObject obj;

    public bool PrepickplayerRandom = false;

    public bool Stopping;
    void Start()
    {
        //picks random player and gets their angle
        randomint = Random.Range(0, numPlayers + 1);
        stopAngle = (360 / numPlayers) * randomint;
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Stopping = !Stopping;

        }

        //starts the slowdown
        if (Stopping == true)
        {
            BaserotationSpeed = BaserotationSpeed - SlowdownSpeed;
        }

        stopAngle = (360 / numPlayers) * randomint;
        obj.transform.Rotate(0, 0, BaserotationSpeed * Time.deltaTime, Space.Self);


        //uses random range to pick player and stop the picker on that player
        if (BaserotationSpeed <= 5 && PrepickplayerRandom == true)
        {
            BaserotationSpeed = 0;
            Quaternion myRot = obj.transform.rotation;
            Quaternion targetRot = myRot;
            targetRot.eulerAngles = new Vector3(0f, 0f, stopAngle + offsetAngleBy);
            obj.transform.rotation = Quaternion.Lerp(myRot, targetRot, Time.deltaTime);//uhh this rotates backwards which makes it look willy wonky

        }



        //stops naturally 
        if (BaserotationSpeed <= 0)
        { Stopping = false; BaserotationSpeed = 0; Debug.Log(obj.transform.rotation.eulerAngles.z); }
        /*Making the player sections(area between 2 angles) 
        playerAng=(360 / numplayers);
        for (int i = 1; i <= numplayers i++)
        {
            minangleforplayer[i] = playerAng * i;
            maxangleforplayer[i] = playerAng * i + 1;
        }
        */
    }

}
