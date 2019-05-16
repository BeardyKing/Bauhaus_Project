using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour{

	PlayerManager pm;
	public GameObject mainCamera;

    void Start(){
		pm = FindObjectOfType<PlayerManager>();
    }
	float zPos = -29;
	float zOffset;
	int lastSpeed;

	float cameraShakeTime;

	Vector3 endPos;

    void Update(){
		if (StaticData.GameRunning == true) {
			zOffset = pm.timeBettweenChange + zPos;

			endPos.x = Mathf.Lerp(mainCamera.transform.position.x, pm.PlayerPosition.x, .35f * Time.deltaTime);
			endPos.y = Mathf.Lerp(mainCamera.transform.position.y, -pm.PlayerPosition.y, .75f * Time.deltaTime);
			endPos.z = -29f;
			mainCamera.transform.position = endPos;

			if (pm.timeBettweenChange > lastSpeed) {
				intensity = pm.timeBettweenChange - lastSpeed;
				cameraShakeTime = 0.15f;
			}

			lastSpeed = pm.timeBettweenChange;
			CameraShake();
		}
	}

	float intensity;

	void CameraShake() {
		if (cameraShakeTime > 0) {
			cameraShakeTime -= Time.deltaTime;
			float rnd = Random.Range(-.5f, .5f);
			mainCamera.transform.eulerAngles = new Vector3(0, 0, rnd);
		} else {
			mainCamera.transform.eulerAngles = new Vector3(0, 0, 0);
		}
	}
}
