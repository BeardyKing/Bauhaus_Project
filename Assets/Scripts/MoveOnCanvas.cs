using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnCanvas : MonoBehaviour {
	int buffer = 0;
	int maxBuffer = 7;
	bool running = false;
	public int[] pPos;
	int x;
	int y;
	int lastX = 0, lastY = 0;
	bool w, a, s, d;
	int[] dir = new int[2];
	int[] lastDir = new int[2];
	bool hasInput;

	int length = 1;
	public int MaxLength = 16;
	int startMaxLen = 15;
	int startSpeed = 20;

	void Start() {
		last4Pos = new List<int[]>();
		last4Pos.Capacity = 4;
	}

	List<int[]> allPositions = new List<int[]>();
	bool reset = false;

	void Update() {
		allPositions.Capacity = length;
		KeyboardController();

		buffer++;
		if (buffer > maxBuffer || reset == true) {
			if (buffer == maxBuffer + 1 || reset == true) {
				reset = false;

				allPositions = new List<int[]>();
				last4Pos = new List<int[]>();
				last4Pos.Capacity = 4;

				int[] temp = new int[2];

				last4Pos.Insert(0, pPos);
				last4Pos.Insert(1, pPos);
				last4Pos.Insert(2, pPos);
				last4Pos.Insert(3, pPos);

				timeBettweenChange = startSpeed;
				length = 1;
				MaxLength = startMaxLen;
				running = true;
				pPos = StaticData.PlayerPos;

				y = pPos[0];
				x = pPos[1];
				allPositions.Add(pPos);

				SeeListContents();
				lastX = x;
				lastY = y;
			}
		}
		if (running == true) {
			MoveUp();
		}
	}

	List<int[]> last4Pos;
	int amountInList = 0;

	void MoveUp() {
		SetNewPosition();
	}
	bool hasMoved = false;

	int counter;
	int timeBettweenChange = 12;
	int minTimeBetweenChange = 8;

	public bool checkedOnce = false;
	void SetNewPosition() {
		// move up y
		counter++;
		if (counter >= timeBettweenChange) {
			if (StaticData.GameObjectList[x + dir[0], y + dir[1]].GetComponent<ChangeColour>().startCol == 1) {
				// made a dir change

				StaticData.GameObjectList[x + dir[0], y + dir[1]].GetComponent<ChangeColour>().startCol = 4;
				hasMoved = true;
				RemoveLastPosition();
				x = x + dir[0];
				y = y + dir[1];
				checkedOnce = false;

			} else if (StaticData.GameObjectList[x + dir[0], y + dir[1]].GetComponent<ChangeColour>().startCol == 4) { // collided with self
				if (checkedOnce == false) {
					checkedOnce = true;
					if (length > 2) {
						bool tempCheck = false;
						if (tempCheck == false) {
							tempCheck = CheckIfSquare();
						}
						AddToListArr(new Vector2(dir[0], dir[1]));
						List4LastPos();
						if (tempCheck == false) {
							CheckIfSquare();
						}
					}
				}

				StaticData.GameObjectList[x + dir[0], y + dir[1]].GetComponent<ChangeColour>().startCol = 4;
				hasMoved = true;
				RemoveLastPosition();
				x = x + dir[0];
				y = y + dir[1];

			} else {
				checkedOnce = false;
				reset = true;
				hasMoved = false;
				pPos[0] = y;
				pPos[1] = x;
				for (int i = 0; i < length - 1; i++) {
					StaticData.GameObjectList[allPositions[i][1], allPositions[i][0]].GetComponent<ChangeColour>().startCol = 1;
				}
			}
			counter = 0;
		}
		SeeListContents();
	}


	bool debBugPass = true;
	int[] colOfObj;

	bool CheckIfSquare() {
		topLeft = Vector2.zero;
		bottomRight = Vector2.zero;
		colOfObj = new int[4];

		Vector2[] pos = new Vector2[4]; // TODO MAKE DYNAMIC
		Vector2 centroid;
		pos[0].x = last4Pos[0][0];
		pos[0].y = last4Pos[0][1];
		
		pos[1].x = last4Pos[1][0];
		pos[1].y = last4Pos[1][1];
		
		pos[2].x = last4Pos[2][0];
		pos[2].y = last4Pos[2][1];
		
		pos[3].x = last4Pos[3][0];
		pos[3].y = last4Pos[3][1];
		
		centroid = (pos[0] + pos[1] + pos[2] + pos[3]);
		centroid = centroid / 4;
		GameObject centroidObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		centroidObj.transform.position = new Vector3(centroid.x, -centroid.y, -1);
		centroidObj.name = "CENTROID";



		for (int i = 0; i < 4; i++) {
			if (pos[i].y > centroid.y) {
				pos[i].y = pos[i].y - 1;

			} else {
				pos[i].y = pos[i].y + 1;
			}

			if (pos[i].x > centroid.x) {
				pos[i].x = pos[i].x - 1;
			} 

			else {
				pos[i].x = pos[i].x + 1;
			}
			colOfObj[i] = StaticData.GameObjectList[(int)pos[i].x, (int)pos[i].y].GetComponent<ChangeColour>().startCol;
			StaticData.GameObjectList[(int)pos[i].x,(int)pos[i].y].GetComponent<ChangeColour>().startCol = 5;

		}

		if ((int)pos[0].x == (int)pos[1].x || pos[0].y == pos[1].y) {
			if (pos[0].x == pos[3].x || pos[0].y == pos[3].y) {
				if (pos[2].x == pos[1].x || pos[2].y == pos[1].y) {
					if (pos[2].x == pos[3].x || pos[2].y == pos[3].y) {
						// SORT VALS

						for (int i = 0; i < 4; i++) {
							if (pos[i].y < centroid.y && pos[i].x < centroid.x) {
								topLeft = pos[i];

							} else if (pos[i].y > centroid.y && pos[i].x > centroid.x) {
								bottomRight = pos[i];
							}
						}

						for (int i = (int)topLeft.x; i < (int)bottomRight.x + 1; i++) {
							for (int j = (int)topLeft.y; j < (int)bottomRight.y + 1 ; j++) {
								if (StaticData.GameObjectList[i, j].GetComponent<ChangeColour>().startCol == 1 ||
									StaticData.GameObjectList[i, j].GetComponent<ChangeColour>().startCol == 2 ||
									StaticData.GameObjectList[i, j].GetComponent<ChangeColour>().startCol == 3 ||
									StaticData.GameObjectList[i, j].GetComponent<ChangeColour>().startCol == 4) {
									for (int c = 0; c < 4; c++) {
										StaticData.GameObjectList[(int)pos[c].x, (int)pos[c].y].GetComponent<ChangeColour>().startCol = colOfObj[c];
									}
									Destroy(centroidObj);
									return false;
								} 
							}
						}

						for (int i = (int)topLeft.x; i < (int)bottomRight.x + 1; i++) {
							for (int j = (int)topLeft.y; j < (int)bottomRight.y + 1; j++) {
								StaticData.GameObjectList[i, j].GetComponent<ChangeColour>().startCol = 7;
							}
						}

						DebugCorners(pos, centroid);
						return true;
					}
				}
			}
		} 
		for (int i = 0; i < 4; i++) {
			StaticData.GameObjectList[(int)pos[i].x, (int)pos[i].y].GetComponent<ChangeColour>().startCol = colOfObj[i];
		}
		Destroy(centroidObj);
		return false;
	}

	void DebugCorners(Vector2[] pos, Vector2 centroid) {
		for (int i = 0; i < 4; i++) {
			StaticData.GameObjectList[(int)pos[i].x, (int)pos[i].y].GetComponent<ChangeColour>().startCol = 5;
		}
	}

	Vector2 topLeft;
	Vector2 bottomRight;

	bool firstChange = false;

	void RemoveLastPosition() {
		if (hasMoved == true) {
			if (x + dir[0] != x || y + dir[1] != y) {
				if (length < MaxLength) {
					Growing();
				} else {
					MaxLen();
				}
				int tempX = allPositions[length - 1][0];
				int tempY = allPositions[length - 1][1];
				StaticData.GameObjectList[tempY, tempX].GetComponent<ChangeColour>().startCol = 1;

				for (int i = 0; i < length - 1; i++) {
					StaticData.GameObjectList[allPositions[i][1], allPositions[i][0]].GetComponent<ChangeColour>().startCol = 4;
				}
			}
		}
	}

	int increaseSpeedCounter = 0;
	int increaseSpeedMaxCounter = 2;

	void Growing() {
		length++;
		int[] temp = new int[2];
		temp[1] = x;
		temp[0] = y;
		if (timeBettweenChange > minTimeBetweenChange) {
			increaseSpeedCounter++;
			if (increaseSpeedCounter >= increaseSpeedMaxCounter) {
				increaseSpeedCounter = 0;
				timeBettweenChange--;
			}
		}
		// increase speed
		if (firstChange == false) {
			firstChange = true;
			length--;
		} else {
			allPositions.Insert(0, temp);
		}
	}

	bool increaseLength = false;
	int lengthClamp = 45;

	void MaxLen() {
		int[] temp = new int[2];
		temp[1] = x;
		temp[0] = y;

		if (increaseLength == true) {
			increaseLength = false;
			if (MaxLength < lengthClamp) {
				if (sameDir == false) {
					MaxLength++;
					Growing();
				}
			}
		}

		allPositions.RemoveAt(MaxLength - 1);
		allPositions.Insert(0, temp);
	}

	void KeyboardController() {

		KeyDown();

		// 0 = x | 1 = y //

		if (w == true) {
			dir[0] = 0; dir[1] = 1;
			hasInput = true;

		} else if (a == true) {
			dir[0] = -1; dir[1] = 0;
			hasInput = true;

		} else if (s == true) {
			dir[0] = 0; dir[1] = -1;
			hasInput = true;

		} else if (d == true) {
			dir[0] = 1; dir[1] = 0;
			hasInput = true;
		} else {
			dir[0] = 0; dir[1] = 0;
			hasInput = false;
		}
	}

	bool sameDir;
	bool hasSameValues = false;

	void AddToListArr(Vector2 tempDir) {
		hasSameValues = false;

		int[] temp = new int[2];
		int[] temp2 = new int[2];

		temp[0] = x + (int)tempDir.x;
		temp[1] = y + (int)tempDir.y;

		try {
			temp2[0] = last4Pos[0][0];
			temp2[1] = last4Pos[0][1];
			if (temp == temp2) {
				hasSameValues = true;
			}
		} catch (System.Exception ex) {
			Debug.LogWarning("err @ AddToListArr : likely issue last4Pos[0][0] is out or range or NULL : ex: " + ex);
		}

		if (hasSameValues == false) {
			last4Pos.Insert(0, temp);
		}
		
		if (last4Pos.Capacity > 4) {
			last4Pos.RemoveAt(4);
			last4Pos.Capacity = 4;
		}
		Debug.Log("added new val");
	}

	bool checkInputOfPlayerAgainstMap(Vector2 inputDir) {

		if (StaticData.GameObjectList[x + (int)inputDir.x, y + (int)inputDir.y].GetComponent<ChangeColour>().startCol != 1 &&
			StaticData.GameObjectList[x + (int)inputDir.x, y + (int)inputDir.y].GetComponent<ChangeColour>().startCol != 4) {
			Debug.Log(" POS [ " + (x + (int)inputDir.x) + " , " + (y + (int)inputDir.y) + " ]" + StaticData.GameObjectList[x + (int)inputDir.x, y + (int)inputDir.y].GetComponent<ChangeColour>().startCol);
			return false;
		} else {
			return true;
		}
	}

	void KeyDown() {
		if (Input.GetKeyDown(KeyCode.W) == true || Input.GetKeyDown(KeyCode.UpArrow) == true) {

			increaseLength = true;
			if (s == true) {
				sameDir = true;
			} else {
				sameDir = false;
			}

			bool canMakeInput = false;
			canMakeInput = checkInputOfPlayerAgainstMap(new Vector2(0,-1));
			if (canMakeInput) {
				if (sameDir == false) {
					AddToListArr(Vector2.zero);
				}

				s = true;

				w = false;
				a = false;
				d = false;
			}


		} else if (Input.GetKeyDown(KeyCode.A) == true || Input.GetKeyDown(KeyCode.LeftArrow) == true) {

			increaseLength = true;
			if (a == true) {
				sameDir = true;
			} else {
				sameDir = false;
			}

			bool canMakeInput = false;
			canMakeInput = checkInputOfPlayerAgainstMap(new Vector2(-1, 0));
			if (canMakeInput) {
				if (sameDir == false) {
					AddToListArr(Vector2.zero);
				}

				a = true;

				s = false;
				w = false;
				d = false;
			}

		} else if (Input.GetKeyDown(KeyCode.S) == true || Input.GetKeyDown(KeyCode.DownArrow) == true) {

			increaseLength = true;
			if (w == true) {
				sameDir = true;
			} else {
				sameDir = false;
			}

			bool canMakeInput = false;
			canMakeInput = checkInputOfPlayerAgainstMap(new Vector2(0, 1));
			if (canMakeInput) {
				if (sameDir == false) {
					AddToListArr(Vector2.zero);
				}

				w = true;

				s = false;
				a = false;
				d = false;
			}

		} else if (Input.GetKeyDown(KeyCode.D) == true || Input.GetKeyDown(KeyCode.RightArrow) == true) {

			increaseLength = true;
			if (d == true) {
				sameDir = true;
			} else {
				sameDir = false;
			}

			bool canMakeInput = false;
			canMakeInput = checkInputOfPlayerAgainstMap(new Vector2(1, 0));

			if (canMakeInput) {
				if (sameDir == false) {
					AddToListArr(Vector2.zero);
				}

				d = true;

				s = false;
				w = false;
				a = false;
			}
		}
	}

	void SeeListContents() {
		if (firstChange == true) {
				if (Input.GetButtonDown("Fire1")) {
				Debug.Log("---- AllPositions ----");
				for (int i = 0; i < length; i++) {
					for (int j = 0; j < 1; j++) {
						Debug.Log("Pos [" + i + "] " + allPositions[i][j] +" : " + allPositions[i][j + 1]);
					}
				}
				List4LastPos();
			}
		}
	}

	void List4LastPos() {
		Debug.Log("---- Last 4 Pos ----");
		for (int i = 0; i < last4Pos.Capacity; i++) {
			for (int j = 0; j < 1; j++) {
				Debug.Log("logged corners [" + i + "] " + last4Pos[i][j] + " : " + last4Pos[i][j + 1]);
			}
		}
		Debug.Log("--------");
	}
}
