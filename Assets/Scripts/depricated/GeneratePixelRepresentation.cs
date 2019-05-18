using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePixelRepresentation : MonoBehaviour
{
	public GameObject px;
	public CountBlockTypes cbt;
	GameObject[] pixles = new GameObject[672];
	Vector3 sv =  new Vector3(-0.58f, -29.87f, -.3f);
	float offset = 0.0599f;
    // Start is called before the first frame update\

    void Start(){
		cbt = FindObjectOfType<CountBlockTypes>();
		for (int i = 0; i < 672; i++) {
			pixles[i] = Instantiate(px, new Vector3(sv.x + (offset * xCount), sv.y + yoffset, sv.z), Quaternion.identity);
			xCount++;
			//if (xCount > xCountMax) {
			//	xCount = 0;
			//	yoffset -= offset;
			//}
		}
	}
	int xCount = 0;
	int xCountMax = 15;

	float yoffset = 0;
    // Update is called once per frame
    void Update()
    {
		for (int i = 0; i < 672; i++) {
			//pixles[i];
			//pixles[i + cbt.colAmount[1] + cbt.colAmount[2]].gameObject.GetComponent<ChangeColour>().startCol = 3;


			if (cbt.colAmount[1] > i) {
				pixles[i].gameObject.GetComponent<ChangeColour>().startCol = 1;
			}
			if (cbt.colAmount[2] > i) {
				pixles[i + cbt.colAmount[1]].gameObject.GetComponent<ChangeColour>().startCol = 2;
			}
			if (cbt.colAmount[3] > i) {
				pixles[i + cbt.colAmount[1] + cbt.colAmount[2]].gameObject.GetComponent<ChangeColour>().startCol = 3;
			}
		}
	}
}
