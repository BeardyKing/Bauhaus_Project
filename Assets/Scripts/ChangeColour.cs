using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
	public int startCol;
	int Buffer;
	public Material[] materials;
	bool running = false;
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
				running = true;
			}
		}
		ReallyLazyUpdateColour();
	}

	void ReallyLazyUpdateColour() {
		if (running == true) {

			UpdateCol();
		}
	}



	void UpdateCol() {
		GetComponent<Renderer>().material = materials[startCol];
	}
}
