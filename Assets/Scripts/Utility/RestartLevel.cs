using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class RestartLevel : MonoBehaviour{
    void Update(){
		if (Input.GetKey(KeyCode.R)) {
			if (StaticData.GameRunning == false) {
				WipeSingletons();
				SceneManager.LoadScene(1);
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 0) {
			if (Input.GetKey(KeyCode.P)) {
				SceneManager.LoadScene(1);
			}
		}
	}

	void WipeSingletons() {
		StaticData.GameRunning = true;
	}

}
