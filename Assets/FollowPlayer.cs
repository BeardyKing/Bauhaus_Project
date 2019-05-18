using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	PlayerManager pm;
	public GameObject mainCamera;



	void Start() {
		pm = FindObjectOfType<PlayerManager>();
	}
	public float zPos = -24;
	public float zOffset;
	int lastSpeed = 1;

	[SerializeField]
	float cameraShakeTime;

	Vector3 endPos;
	public float crashOffset;
	int gameStartBuffer = 15;
	float posXoffset;
	float posYoffset;
	public float xVal = 0;
	public float yVal = 0;
	float zVal = 0;


	void Update() {
		if (gameStartBuffer > 0) {
			gameStartBuffer--;
		}
		else{
			if (StaticData.GameRunning == true) {
				zOffset = pm.timeBettweenChange + zPos + crashOffset;

				endPos.x = Mathf.Lerp(mainCamera.transform.position.x , pm.PlayerPosition.x + posXoffset, .7f * Time.deltaTime);
				endPos.y = Mathf.Lerp(mainCamera.transform.position.y , -pm.PlayerPosition.y+ posYoffset, .8f * Time.deltaTime);
				endPos.z = Mathf.Lerp(mainCamera.transform.position.z , zOffset + zVal, .35f * Time.deltaTime);
				mainCamera.transform.position = endPos;

				if (pm.timeBettweenChange > lastSpeed) {
					intensity = (pm.timeBettweenChange / lastSpeed) / 30;
					cameraShakeTime = 0.04f + intensity;
				}

				lastSpeed = pm.timeBettweenChange;
				CameraShakeRotation();
				CameraSpeedOffset();
			} else {
				mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.Euler(Vector3.zero), 2f * Time.deltaTime);
			}
		}
	}

	float rotationSpeedOffset = 4;
	Vector3 targetRotationOffset;
	Vector3 rotOut;
	void CameraSpeedOffset() {

		if (pm.dir[0] == 0 && pm.dir[1] == 1) { // w
			targetRotationOffset = new Vector3(rotationSpeedOffset, 0, zRot);
			posYoffset = -yVal;
		}
		if (pm.dir[0] == 0 && pm.dir[1] == -1) { // s
			targetRotationOffset = new Vector3(-rotationSpeedOffset, 0, zRot);
			posYoffset = yVal;

		}
		if (pm.dir[0] == -1 && pm.dir[1] == 0) { // a
			targetRotationOffset = new Vector3(0, -rotationSpeedOffset, zRot);
			posXoffset = -xVal;

		}
		if (pm.dir[0] == 1 && pm.dir[1] == 0) { // d
			targetRotationOffset = new Vector3(0, rotationSpeedOffset, zRot);
			posXoffset = xVal;

		}
		if (pm.dir[0] == 0 && pm.dir[1] == 0) {
			targetRotationOffset = new Vector3(0, 0, 0);
			posXoffset = 0;
			posYoffset = 0;


		}
		mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.Euler(targetRotationOffset), 2f * Time.deltaTime);
	}
	[SerializeField]
	float intensity;
	Vector3 rndPos;
	float shakeOffset;
	float rndRotation;
	float zRot;

	void CameraShakeRotation() {
		if (cameraShakeTime > 0) {
			crashOffset = 120;
			cameraShakeTime -= (Time.deltaTime);

			rndRotation = Random.Range(-4f, 4f);
			mainCamera.transform.rotation = Quaternion.Euler(new Vector3(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, rndRotation));
			//mainCamera.transform.eulerAngles = new 

			shakeOffset = ((cameraShakeTime) / Time.deltaTime) / 35;
			rndPos = new Vector3(Random.Range(-shakeOffset,shakeOffset), Random.Range(-shakeOffset, shakeOffset), Random.Range(-shakeOffset, shakeOffset));

			//rndPos = new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));
			//mainCamera.transform.position = mainCamera.transform.position + rndPos;
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(pm.PlayerPosition.x, -pm.PlayerPosition.y, -6), 4 * Time.deltaTime) ;
			zRot = rndRotation;

		} 
		else {
			//crashOffset = 0;
			//if (crashOffset > 0) {
			//	crashOffset += Time.deltaTime * 2;
			//}
			crashOffset = crashOffset / 2;
			zRot = Mathf.Lerp(zRot, 0, 1f * Time.deltaTime);
			//mainCamera.transform.eulerAngles = new Vector3(0, 0, 0);
		}
	}
}
