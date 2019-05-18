using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEndLevelController : MonoBehaviour
{
	public GameObject mainCam;

	public Vector3 endGamePosNodes;
    void Start(){
		//endGamePosNodes = new Vector3(18.5f, -14.5f, -664.7f);
	}

    void Update(){
		if (Input.GetKey(KeyCode.P)) {
			StaticData.GameRunning = false;
		}

		if (StaticData.GameRunning == false) {
			mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, endGamePosNodes, .09f * Time.deltaTime);
		}
	}
}
