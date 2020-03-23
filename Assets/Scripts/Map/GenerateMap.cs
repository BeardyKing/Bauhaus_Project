using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
	int twoFrames = 0;
	int xLen;
	int yLen;
	public ReadMapRaw readMapRaw;
	public GameObject block;
	GameObject[,] blocks2D = new GameObject[1000,1000]; // need a dynamic solution for final
	int[,] mapRef;
    // Start is called before the first frame update
    void Start()
    {
		Application.targetFrameRate = 60;
		//CreateMap();
	}

	// Update is called once per frame
	void Update()
    {
		if (twoFrames < 2) {
			twoFrames++;
			//Debug.Log(twoFrames);
			if (twoFrames >= 2) {
				xLen = readMapRaw.x;
				yLen = readMapRaw.y;
				mapRef = StaticData.Map;
				CreateMap();

				//StaticData.GameRunning = true; TODO WAITING FOR IMPLEMENTATION 
				StaticData.GameObjectList = blocks2D;
			}
		}
	}

	void CreateMap() {
//		Debug.Log("hi");
		for (int i = 0; i < xLen; i++) {
			for (int j = 0; j < yLen; j++) {
				blocks2D[i, j] = Instantiate(block, new Vector3(i, -j, 0), Quaternion.identity);
				//blocks2D[i, j].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
				blocks2D[i,j].name = "" + i + "," + j;
				blocks2D[i,j].GetComponent<ChangeColour>().startCol = mapRef[j,i];
			}
		}
	}

}
