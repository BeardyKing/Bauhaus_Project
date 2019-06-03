using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEndLevelController : MonoBehaviour
{
	public GameObject mainCam;

	private  Vector3 endGamePosNodes;
    void Start(){
		endGamePosNodes = new Vector3(19.7f, -13f, -100);
	}

    void Update(){
		if (Input.GetKey(KeyCode.P)) {
			StaticData.GameRunning = false;
		}

		if (StaticData.GameRunning == false) {
            if (mainCam.transform.position.z > -100)
            {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, endGamePosNodes, .08f * Time.deltaTime);
            }
		}
	}
}
