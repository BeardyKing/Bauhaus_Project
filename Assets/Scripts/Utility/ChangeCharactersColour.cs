using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharactersColour : MonoBehaviour
{
	public CountBlockTypes cbt;
	public Material[] mats;
	public GameObject[] characters;
	int matChoice = 0;
	bool onePass = false;

    // Start is called before the first frame update
    void Start()
    {
		//FindObjectOfType<CountBlockTypes>();
    }

	// Update is called once per frame
    void Update(){
	
				if (cbt.rWin == true) {
			characters[0].gameObject.GetComponent<Renderer>().material = mats[0];
			characters[1].gameObject.GetComponent<Renderer>().material = mats[0];
			characters[2].gameObject.GetComponent<Renderer>().material = mats[0];
			characters[3].gameObject.GetComponent<Renderer>().material = mats[0];
		}
				if (cbt.bWin == true) {
			characters[0].gameObject.GetComponent<Renderer>().material = mats[1];
			characters[1].gameObject.GetComponent<Renderer>().material = mats[1];
			characters[2].gameObject.GetComponent<Renderer>().material = mats[1];
			characters[3].gameObject.GetComponent<Renderer>().material = mats[1];
		}
				if (cbt.yWin == true) {
			characters[0].gameObject.GetComponent<Renderer>().material = mats[2];
			characters[1].gameObject.GetComponent<Renderer>().material = mats[2];
			characters[2].gameObject.GetComponent<Renderer>().material = mats[2];
			characters[3].gameObject.GetComponent<Renderer>().material = mats[2];
		}
	}
}
