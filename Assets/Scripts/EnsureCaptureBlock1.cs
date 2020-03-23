using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnsureCaptureBlock1 : MonoBehaviour
{
    int startCounter = 15;
    public Vector2Int[] topL_botR= new Vector2Int[2];
    public Vector2Int[] corners;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector2 topLeft;
    Vector2 bottomRight;
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K)) {
        //    for (int z = 0; z < corners.Length; z++) {
        //        StaticData.GameObjectList[corners[z].x, corners[z].y].GetComponent<ChangeColour>().startCol = 4;
        //    }
        //}
        if (startCounter > 0) {
            startCounter--;
        }
        else {
            for (int i = 0; i < corners.Length; i++) {
                if (StaticData.GameObjectList[corners[i].x, corners[i].y].GetComponent<ChangeColour>().startCol == 4) {
                    doCapture = true;
                }
                else {
                    doCapture = false;
                    break;
                }
            }

            if (doCapture == true) {
                topLeft = topL_botR[0];
                bottomRight = topL_botR[1];
                for (int i = (int)topLeft.x; i < (int)bottomRight.x + 1; i++) {
                    for (int j = (int)topLeft.y; j < (int)bottomRight.y + 1; j++) {
                        StaticData.GameObjectList[i, j].GetComponent<ChangeColour>().startCol = 7;
                    }
                }
            }
        }
    }
    public bool doCapture = false;
    public int amountCounter;
}
