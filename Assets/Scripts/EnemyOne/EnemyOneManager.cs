using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOneManager : MonoBehaviour {
	[Header("Reference Data")]

	public Vector2Int PlayerPosition;
	List<int[]> last4Pos;
	List<int[]> allPositions = new List<int[]>();



	int 			buffer;
	readonly int 	maxBuffer = 7;

	bool 			running;
	bool 			firstChange;
	bool 			increaseLength;



	readonly int[] dir = new int[2];

	public int[] PlayerDir {
		get {
			return dir;
		}
	}

	[HideInInspector]
	public int[] 	pPos;

	int 			x,
					y;

	bool 			w,
					a,
					s,
					d;

	[HideInInspector]
	public bool 	left,
					right,
					up,
					down;

	public bool 	hasBuffer;

	bool 			hasMoved;



	[Header("Length - Editable")]

	public int 		MaxLength = 16;
	public int 		startMaxLen = 16;
	readonly int 	lengthClamp = 45;
	int 			length = 1;


	[Header("Speed - Editable")]

	int 			counter;
	int 			increaseSpeedCounter;

	public int 		startSpeed;                  // = 6;
	public int 		timeBettweenChange;          // = 12;
	public int 		minTimeBetweenChange;        // = 8;
	readonly int 	increaseSpeedMaxCounter = 2;

	[HideInInspector]
	public bool 	checkedOnce;

	int[] 			colOfObj;

	Vector2 		topLeft;
	Vector2 		bottomRight;



	float 			currentCounter;
	bool 			startTimer;

	bool 			singlepass;
	bool 			sameDir;
	bool 			hasSameValues;
	bool 			reset;





	void Start() {
		last4Pos = new List<int[]> { Capacity = 4 };
	}

	void Update() {
		if (StaticData.GameRunning == true) {
			PopulateArrays();

			allPositions.Capacity = length;

			if (running == true) {
				if (hasBuffer == true) {
					BufferedInput(left, right, up, down);
				}
				SetNewPosition();
				PlayerPosition = new Vector2Int(x, y);
			}
		}
	}

	void PopulateArrays() {
		if (buffer <= maxBuffer + 2) {
			buffer++;
		}

		if (buffer > maxBuffer || reset == true) {
			if (buffer == maxBuffer + 1 || reset == true) {
				reset = false;

				allPositions = new List<int[]>();
				last4Pos = new List<int[]> { Capacity = 4 };

				last4Pos.Insert(0, pPos);
				last4Pos.Insert(1, pPos);
				last4Pos.Insert(2, pPos);
				last4Pos.Insert(3, pPos);

				timeBettweenChange = startSpeed;
				length = 1;
				MaxLength = startMaxLen;
				running = true;
				pPos = StaticData.EnemyOnePos; // TODO DEPRICATED

				y = pPos[0];
				x = pPos[1];
				allPositions.Add(pPos);

				//SeeListContents(); // DEBUG CORNERS
			}
		}
	}
	#region Input

	public void PushInput(Vector2 input, float bufferTimer) {
		if (hasBuffer == false) {
			if (input == Vector2.up) {
				up = true;

				down = false;
				left = false;
				right = false;
				hasBuffer = true;
			}
			if (input == Vector2.down) {
				down = true;

				up = false;
				right = false;
				left = false;
				hasBuffer = true;

			}
			if (input == Vector2.left) {
				left = true;

				up = false;
				down = false;
				right = false;
				hasBuffer = true;

			}
			if (input == Vector2.right) {
				right = true;

				up = false;
				down = false;
				left = false;
				hasBuffer = true;

			}
		}
		if (hasBuffer == true) {
			startTimer = true;
		}

		if (startTimer == true) {
			if (singlepass == false) {
				singlepass = true;
				currentCounter = bufferTimer;
			}
			currentCounter -= Time.deltaTime;
			if (currentCounter <= 0) {
				hasBuffer = false;
				singlepass = false;
				startTimer = false;
			}
		}
	}

	#endregion

	void SetNewPosition() {
		counter++;
		if (counter >= timeBettweenChange) {
			if (StaticData.GameObjectList[x + dir[0], y + dir[1]].GetComponent<ChangeColour>().startCol == 1) {
				OnPath();
			} else if (StaticData.GameObjectList[x + dir[0], y + dir[1]].GetComponent<ChangeColour>().startCol == 2) {
				CollidedWithSelf();
			} else {
				CollidedWithWall();
			}
			counter = 0;
		}
		SeeListContents();
	}

	#region Collisions

	void OnPath() {
		StaticData.GameObjectList[x + dir[0], y + dir[1]].GetComponent<ChangeColour>().startCol = 2;
		hasMoved = true;
		RemoveLastPosition();
		x = x + dir[0];
		y = y + dir[1];
		checkedOnce = false;
	}

	public bool hasCapturedSquare;

	void CollidedWithSelf() {
		if (checkedOnce == false) {
			checkedOnce = true;
			if (length > 2) {
				bool tempCheck = false;
				if (tempCheck == false) {
					tempCheck = CheckIfSquare();
				}

				AddToListArr(new Vector2(dir[0], dir[1]));

				if (tempCheck == false) {
					CheckIfSquare();

				}
				hasCapturedSquare = tempCheck;
			}
		}

		StaticData.GameObjectList[x + dir[0], y + dir[1]].GetComponent<ChangeColour>().startCol = 2;
		hasMoved = true;
		RemoveLastPosition();
		x = x + dir[0];
		y = y + dir[1];
	}

	void CollidedWithWall() {
		checkedOnce = false;
		reset = true;
		hasMoved = false;
		pPos[0] = y;
		pPos[1] = x;
		dir[0] = 0;
		dir[1] = 0;
		for (int i = 0; i < length - 1; i++) {
			StaticData.GameObjectList[allPositions[i][1], allPositions[i][0]].GetComponent<ChangeColour>().startCol = 1;
		}
	}
	#endregion

	bool CheckIfSquare() {
		try {
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
			//GameObject centroidObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			GameObject centroidObj = new GameObject(); // TODO CHANGED FROM SPHERE CHANGE BACK IF THIS BREAKS ANYTHING

			centroidObj.transform.position = new Vector3(centroid.x, -centroid.y, -1);
			centroidObj.name = "CENTROID FROM ENEMY ONE";

			for (int i = 0; i < 4; i++) {
				if (pos[i].y > centroid.y) {
					pos[i].y = pos[i].y - 1;

				} else {
					pos[i].y = pos[i].y + 1;
				}

				if (pos[i].x > centroid.x) {
					pos[i].x = pos[i].x - 1;
				} else {
					pos[i].x = pos[i].x + 1;
				}
				colOfObj[i] = StaticData.GameObjectList[(int)pos[i].x, (int)pos[i].y].GetComponent<ChangeColour>().startCol;
				//StaticData.GameObjectList[(int)pos[i].x, (int)pos[i].y].GetComponent<ChangeColour>().startCol = 5; // TODO REMOVE IF EVERYTHING BREAKS

			}

			if ((int)pos[0].x == (int)pos[1].x || (int)pos[0].y == (int)pos[1].y) {
				if ((int)pos[0].x == (int)pos[3].x || (int)pos[0].y == (int)pos[3].y) {
					if ((int)pos[2].x == (int)pos[1].x || (int)pos[2].y == (int)pos[1].y) {
						if ((int)pos[2].x == (int)pos[3].x || (int)pos[2].y == (int)pos[3].y) {
							// SORT VALS

							for (int i = 0; i < 4; i++) {
								if (pos[i].y < centroid.y && pos[i].x < centroid.x) {
									topLeft = pos[i];

								} else if (pos[i].y > centroid.y && pos[i].x > centroid.x) {
									bottomRight = pos[i];
								}
							}

							for (int i = (int)topLeft.x; i < (int)bottomRight.x + 1; i++) {
								for (int j = (int)topLeft.y; j < (int)bottomRight.y + 1; j++) {
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
									StaticData.GameObjectList[i, j].GetComponent<ChangeColour>().startCol = 5;
								}
							}

							Destroy(centroidObj);
							//DebugCorners(pos, centroid); // DEBUG
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
		} catch (System.Exception ex) {
			Debug.LogWarning("err @ CheckSquare probably to do with array not being fully populated : " + ex);

			return false;
		}

	}



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
					StaticData.GameObjectList[allPositions[i][1],
						allPositions[i][0]].GetComponent<ChangeColour>().
						startCol = 2;
				}
			}
		}
	}



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
		//Debug.Log("added new val");
	}

	bool CheckInputOfPlayerAgainstMap(Vector2 inputDir) {

		if (StaticData.GameObjectList[x + (int)inputDir.x, y + (int)inputDir.y].GetComponent<ChangeColour>().startCol != 1 &&
			StaticData.GameObjectList[x + (int)inputDir.x, y + (int)inputDir.y].GetComponent<ChangeColour>().startCol != 2) {
			//Debug.Log(" POS [ " + (x + (int)inputDir.x) + " , " + (y + (int)inputDir.y) + " ]" + StaticData.GameObjectList[x + (int)inputDir.x, y + (int)inputDir.y].GetComponent<ChangeColour>().startCol);
			return false;
		} else {
			return true;
		}
	}


	void BufferedInput(bool lf, bool ri, bool upp, bool dn) {
		if (upp == true) {
			increaseLength = true;
			if (s == true) {
				sameDir = true;
			} else {
				sameDir = false;
			}

			bool canMakeInput = false;
			canMakeInput = CheckInputOfPlayerAgainstMap(new Vector2(0, -1));
			if (canMakeInput) {
				if (sameDir == false) {
					AddToListArr(Vector2.zero);
				}

				s = true;

				w = false;
				a = false;
				d = false;
			}
		}
		if (lf == true) {
			increaseLength = true;
			if (a == true) {
				sameDir = true;
			} else {
				sameDir = false;
			}

			bool canMakeInput = false;
			canMakeInput = CheckInputOfPlayerAgainstMap(new Vector2(-1, 0));
			if (canMakeInput) {
				if (sameDir == false) {
					AddToListArr(Vector2.zero);
				}

				a = true;

				s = false;
				w = false;
				d = false;
			}
		}
		if (dn == true) {
			increaseLength = true;
			if (w == true) {
				sameDir = true;
			} else {
				sameDir = false;
			}

			bool canMakeInput = false;
			canMakeInput = CheckInputOfPlayerAgainstMap(new Vector2(0, 1));
			if (canMakeInput) {
				if (sameDir == false) {
					AddToListArr(Vector2.zero);
				}

				w = true;

				s = false;
				a = false;
				d = false;
			}
		}
		if (ri == true) {
			increaseLength = true;
			if (d == true) {
				sameDir = true;
			} else {
				sameDir = false;
			}

			bool canMakeInput = false;
			canMakeInput = CheckInputOfPlayerAgainstMap(new Vector2(1, 0));

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

		// TODO dont allow player to mave back on self

		if (w == true) {
			if (dir[1] != -1) {
				dir[0] = 0; dir[1] = 1;
			}
		} else if (a == true) {
			if (dir[0] != 1) {
				dir[0] = -1; dir[1] = 0;
			}
		} else if (s == true) {
			if (dir[1] != 1) {
				dir[0] = 0; dir[1] = -1;
			}
		} else if (d == true) {
			if (dir[0] != -1) {
				dir[0] = 1; dir[1] = 0;
			}
		} else {
			dir[0] = 0; dir[1] = 0;
		}


	}

	#region debug

	void DebugCorners(Vector2[] pos, Vector2 centroid) {
		// DEBUG SEE CORNERS BEING USED FOR CALCULATION 
		for (int i = 0; i < 4; i++) {
			StaticData.GameObjectList[(int)pos[i].x, (int)pos[i].y].GetComponent<ChangeColour>().startCol = 6;
		}
	}

	void SeeListContents() {
		if (firstChange == true) {
			if (Input.GetButtonDown("Fire1")) {
				Debug.Log("---- AllPositions ----");
				for (int i = 0; i < length; i++) {
					for (int j = 0; j < 1; j++) {
						Debug.Log("Pos [" + i + "] " + allPositions[i][j] + " : " + allPositions[i][j + 1]);
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
	#endregion


	#region Depricated
	void KeyboardController() {

		KeyDown();

		// 0 = x | 1 = y //

		if (w == true) {
			dir[0] = 0; dir[1] = 1;

		} else if (a == true) {
			dir[0] = -1; dir[1] = 0;

		} else if (s == true) {
			dir[0] = 0; dir[1] = -1;

		} else if (d == true) {
			dir[0] = 1; dir[1] = 0;
		} else {
			dir[0] = 0; dir[1] = 0;
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
			canMakeInput = CheckInputOfPlayerAgainstMap(new Vector2(0, -1));
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
			canMakeInput = CheckInputOfPlayerAgainstMap(new Vector2(-1, 0));
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
			canMakeInput = CheckInputOfPlayerAgainstMap(new Vector2(0, 1));
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
			canMakeInput = CheckInputOfPlayerAgainstMap(new Vector2(1, 0));

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
	#endregion
}
