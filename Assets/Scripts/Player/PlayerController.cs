using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	PlayerManager pm;
	Vector2 input;

	[Range(.1f,1)]	
	public float bufferTimer;

	void Start() {
		pm = GetComponent<PlayerManager>();
	}

	void Update(){

        
        input = new Vector2(
				Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical")
				).normalized;

		pm.PushInput(input, bufferTimer);

		if (posDirInputWasMade != pm.PlayerPosition) {
			if (input != Vector2Int.zero) {
				if (pm.sameDir == false) {
					SetDirAndOnePass(new Vector2Int((int)input.x, (int)input.y));
                    onePerSquare = false;
				}
			}
            AddLength();

		}
        lastDir = pm.dir;
	}

	bool canAdd;

	int[] lastDir = new int[2];

	[SerializeField]
	int directionsMade;
	readonly int inputsNeededToGrow = 2;
	public int lengthCap = 7;
	Vector2Int posDirInputWasMade;
    public Vector2Int dirFromPM;

    void AddLength(){
        dirFromPM = new Vector2Int(pm.dir[0], pm.dir[1]);
        lastPredictedDir = predictedDir;

        predictedPosition = LastPlayerPosition + predictedDir;
        PlayerPosition = pm.PlayerPosition;
        LastPlayerPosition = pm.LastPlayerPosition;
        if ((PlayerPosition - LastPlayerPosition) != Vector2Int.zero ) {
            predictedDir = PlayerPosition - LastPlayerPosition;
        }
        //predictedPosition = PlayerPosition + predictedDir;

        if (dirFromPM != lastPredictedDir ) {
            if (pm.length > 2) {
                if (onePerSquare == false) {
                    onePerSquare = true;
                    if (pm.MaxLength <= lengthCap) {
                        if (posDirInputWasMade != pm.PlayerPosition) {
                            pm.MaxLength++;
                            pm.MaxLength++;
                        }
                    }
                    posDirInputWasMade = pm.PlayerPosition;
                }
            }
        }

        //WORKS 1 UNIT OF MOVEMENT LATE
        //if (predictedDir != lastPredictedDir) {
        //    pm.MaxLength++;
        //}
    }
    Vector2Int posInputWasMadeOn;
    bool onePerSquare;
    bool doNextFrame = false;

    public Vector2Int PlayerPosition;
    public Vector2Int LastPlayerPosition;
    public Vector2Int predictedDir;
    public Vector2Int predictedPosition;
    public Vector2Int lastPredictedDir;


    //public Vector2Int NextPlayerPositon;
    //public Vector2Int currentDir;
    //public Vector2Int nextDir;

    Vector2Int CalculateNextPlayerPosition(Vector2Int current)
    {
        Vector2Int Input = new Vector2Int();
        Input.x = input.x == 1 ? 1 : 0;
        Input.y = input.y == 1 ? 1 : 0;

        Vector2Int toReturn = current + Input;

        return new Vector2Int();

    }



    void SetDirAndOnePass(Vector2Int inDir) { // TODO

		directionsMade++;
		if (directionsMade > inputsNeededToGrow) {
			directionsMade = 0;
			if (pm.MaxLength < lengthCap) {

				//pm.MaxLength++;
				//Debug.Log("DOING INPUT");
				canAdd = false;


            }
		}
	}
}


//if (doNextFrame == true) {
//    doNextFrame = false;
//    if (predictedPosition == PlayerPosition) {
//        Debug.Log("IN HERE");
//        if (pm.length != 1) {
//            //pm.MaxLength++;
//        }
//    }
//    Debug.Log("done");
//}


//if (pm.counter >= pm.timeBettweenChange - 1) {
//    doNextFrame = true;

//}

// Get Previous Position?

// Get Current Position

// Compare Positions of Prev and Current

// Read Input

// calculate next pos based on read

// calculate direction based on current and next

// compare 2 directions found 

//if they are the same dont add

//if they are different add one length


//Vector2Int PlayerPosition = pm.PlayerPosition;

//Vector2Int LastPlayerPosition = pm.LastPlayerPosition;

//Vector2Int NextPlayerPositon = CalculateNextPlayerPosition(PlayerPosition);


//Vector2Int currentDir = PlayerPosition - LastPlayerPosition;

//Vector2Int nextDir = NextPlayerPositon - PlayerPosition;


//if(currentDir!= nextDir && currentDir != Vector2Int.zero) {

//     pm.MaxLength++;       
//}

//PlayerPosition = pm.PlayerPosition;

//LastPlayerPosition = pm.LastPlayerPosition;

//NextPlayerPositon = CalculateNextPlayerPosition(PlayerPosition);


//currentDir = PlayerPosition - LastPlayerPosition;

////nextDir = (NextPlayerPositon - PlayerPosition) * -1;
//nextDir = (NextPlayerPositon - PlayerPosition) * -1;
//nextDir = nextDir + currentDir;


//if ((PlayerPosition) != nextDir) {
//    pm.MaxLength++;
//}







