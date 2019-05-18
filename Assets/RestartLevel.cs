using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class RestartLevel : MonoBehaviour
{
    void Update(){
		if (Input.GetKey(KeyCode.R)) {
			if (StaticData.GameRunning == false) {
				SceneManager.LoadScene(1);
			}
		}
	}
}
