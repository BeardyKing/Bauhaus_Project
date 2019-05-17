using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
	public int startCol;
	int Buffer;
	public Material[] materials;
	bool running = false;
	bool isRoad;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Buffer++;
		if (Buffer > 5) {
			if (Buffer == 6) {
				UpdateCol();
				MoveWhiteForwards();
				running = true;
			}
		}
		ReallyLazyUpdateColour();
	}

	void ReallyLazyUpdateColour() {
		if (running == true) {

			UpdateCol();
			if (isRoad) {
				DoPlayerRoadInteraction(); // REALLY HEAVY SCRIPT
			} else {
				DoCaptureInteraction();
			}
		}
	}



	void UpdateCol() {
		GetComponent<Renderer>().material = materials[startCol];
	}

	void MoveWhiteForwards() {
		if (startCol == 0) {
			transform.position = new Vector3(transform.position.x, transform.position.y, -.1f);
		} else {
			isRoad = true;
		}
	}

	void DoPlayerRoadInteraction() {
		if (startCol != 1) {
			transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, -.2f), 5 * Time.deltaTime);
		} else {
			transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, 0f), 2 * Time.deltaTime);
		}
	}
	void DoCaptureInteraction() {
		if (startCol != 0) {
			if (sinAmount < 9999999) {
				sinAmount = sinAmount * Random.Range(1.1f, 1.3f);
				transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Cos(Mathf.PI * ((transform.position.x + transform.position.y) + Time.time)) / sinAmount);
			}
		}
	}
	float sinAmount = 1;
}
