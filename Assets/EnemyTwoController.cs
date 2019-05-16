using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoController : MonoBehaviour {
	// ENEMY COLOUR = RED (2)
	EnemyTwoManager enemyManager;
	Vector2 input;

	[Range(.001f, 1f)]
	public float bufferTimer;


	public Vector2Int[] possibleCapturePositions;
	public Vector2Int[] possibleGoToPositions;

	public Vector2Int randomlySelectedPoint;
	public Vector2Int CapturePos;


	Vector2Int dir = Vector2Int.zero;
	Vector2Int u = Vector2Int.up;
	Vector2Int d = Vector2Int.down;
	Vector2Int l = Vector2Int.left;
	Vector2Int r = Vector2Int.right;

	Vector2Int posDirInputWasMade;

	Vector2Int eom_dir;

	public string state = "rnd";
	string startState = "rnd";

	void Start() {
		enemyManager = GetComponent<EnemyTwoManager>();
	}

	int frameStartBuffer = 10;

	void Update() {
		if (frameStartBuffer > 0) {
			frameStartBuffer--;
		} else {
			MovementFallback();
			MovementExceptions();
			StateController();
			CheckWhenAtIntersection();
			MakeIntersectionInput();

			enemyManager.PushInput(input, bufferTimer);
			if (Input.GetKeyDown(KeyCode.P)) {
				state = "capture";
			}
		}
	}
	[SerializeField]
	float state_timer = 3;

	float state_rnd_maxTimer = 10;
	float state_goto_maxTimer = 10;
	float state_capture_maxTimer = 10;

	float stuckCounter = 0.3f;

	void MovementFallback() {
		//stuckCounter -= Time.deltaTime;
		//if (posDirInputWasMade == enemyManager.PlayerPosition ) {
		//	if (stuckCounter <= 0) {
		//		RandomMovement();
		//		Debug.Log("AAA STUCK");
		//	}
		//}
		//else if(posDirInputWasMade != enemyManager.PlayerPosition) {
		//	stuckCounter = .3f;
		//}

	}

	void MovementExceptions() {
		if (enemyManager.PlayerPosition == new Vector2Int(1, 1)) {
			if (eom_dir == u) {
				if (posDirInputWasMade != enemyManager.PlayerPosition) {
					SetDirAndOnePass(r);
				}
			}
			if (eom_dir == l) {
				if (posDirInputWasMade != enemyManager.PlayerPosition) {
					SetDirAndOnePass(d);
				}
			}
		}

		if (enemyManager.PlayerPosition == new Vector2Int(38, 1)) {
			if (eom_dir == u) {
				if (posDirInputWasMade != enemyManager.PlayerPosition) {
					SetDirAndOnePass(l);
				}
			}
			if (eom_dir == r) {
				if (posDirInputWasMade != enemyManager.PlayerPosition) {
					SetDirAndOnePass(d);
				}
			}
		}

		if (enemyManager.PlayerPosition == new Vector2Int(1, 28)) {
			if (eom_dir == d) {
				if (posDirInputWasMade != enemyManager.PlayerPosition) {
					SetDirAndOnePass(r);
				}
			}
			if (eom_dir == l) {
				if (posDirInputWasMade != enemyManager.PlayerPosition) {
					SetDirAndOnePass(u);
				}
			}
		}

		if (enemyManager.PlayerPosition == new Vector2Int(38, 28)) {
			if (eom_dir == d) {
				if (posDirInputWasMade != enemyManager.PlayerPosition) {
					SetDirAndOnePass(l);
				}
			}
			if (eom_dir == r) {
				if (posDirInputWasMade != enemyManager.PlayerPosition) {
					SetDirAndOnePass(u);
				}
			}
		}
	}

	void StateController() {
		//TODO ADD CHANCE TO CRASH
		if (state == "rnd") {
			state_timer -= Time.deltaTime;
			if (state_timer <= 0) {
				int rndm = Random.Range(0, 3);
				switch (rndm) {
					case 0:
						state = "rnd";
						state_timer = state_rnd_maxTimer;
						Debug.Log(state);
						break;
					case 1:
						state = "goto";
						state_timer = state_goto_maxTimer;
						randomlySelectedPoint = possibleGoToPositions[(int)Random.Range(0, possibleGoToPositions.Length)];
						Debug.Log(state);

						break;
					case 2:
						state = "capture";
						state_timer = state_capture_maxTimer;
						CapturePos = possibleCapturePositions[(int)Random.Range(0, possibleCapturePositions.Length)];
						Debug.Log(state);

						break;
					default:
						break;
				}
			}
		} else if (state == "goto") {
			state_timer -= Time.deltaTime;
			// TODO CHECK WHEN IT GETS NEAR TO TARGET POS
			if (state_timer <= 0) {
				state = "capture";
				state_timer = state_capture_maxTimer;
			}

		} else if (state == "capture") {
			if (enemyManager.hasCapturedSquare) {
				enemyManager.hasCapturedSquare = false;
				Debug.Log("---------------------");
				Debug.Log("HAS CAPTURED A SQUARE");
				Debug.Log("---------------------");

				Capture_ChangeState();
			}
			state_timer -= Time.deltaTime;
			if (state_timer <= 0) {
				Capture_ChangeState();
			}
		}
	}

	void Capture_ChangeState() {
		int rndm = Random.Range(0, 2);
		switch (rndm) {
			case 0:
				state = "rnd";
				state_timer = state_rnd_maxTimer;
				Debug.Log(state);
				break;
			case 1:
				state = "capture";
				state_timer = state_capture_maxTimer;
				CapturePos = possibleCapturePositions[(int)Random.Range(0, possibleCapturePositions.Length)];
				Debug.Log(state);

				break;
			default:
				break;
		}
	}

	void MakeIntersectionInput() {
		if (state == "rnd") {
			RandomMovement();
		} else if (state == "goto") {
			GoToPosition();
		} else if (state == "capture") {
			Capture();
		}
	}


	int directionsMade;
	int inputsNeededToGrow;
	public int lengthCap = 50;

	void SetDirAndOnePass(Vector2Int inDir) {
		input = inDir;
		atIntersection = false;
		posDirInputWasMade = enemyManager.PlayerPosition;
		directionsMade++;
		if (directionsMade > inputsNeededToGrow) {
			directionsMade = 0;
			if (enemyManager.MaxLength < lengthCap) {
				enemyManager.MaxLength++;
			}
		}
	}


	public bool goingToCrash = false;

	void Capture() {
		if (atIntersection == true) {
			if (posDirInputWasMade != enemyManager.PlayerPosition) {
				if (gtp_flipFlop == true) {
					if (enemyManager.PlayerPosition.y < CapturePos.y) {
						if (canMoveDown) {
							SetDirAndOnePass(d);
						}
					} else {
						if (canMoveUp) {
							SetDirAndOnePass(u);
						}
					}
				} else {
					if (enemyManager.PlayerPosition.x < CapturePos.x) {
						if (canMoveRight) {
							SetDirAndOnePass(r);
						}
					} else {
						if (canMoveLeft) {
							SetDirAndOnePass(l);
						}
					}
				}
				gtp_flipFlop = !gtp_flipFlop;
			}
		}
	}



	void SetDirCrashFallBack() {
		RandomMovement();
	}



	//TODO USE THIS AS PART OF THE REVIEW OF THIS UNIT AS HOW WE INCREASED OPTIMISATION
	//void RandomMovement() {
	//	if (atIntersection == true) {
	//for (int i = 0; i<9000; i++) {
	//		int selection = Random.Range(0, 4);
	//		if (posDirInputWasMade != enemyManager.PlayerPosition) {
	//			if 	(canMoveUp 				&& selection == 0) {
	//				SetDirAndOnePass(u);
	//			} else if (canMoveDown 		&& selection == 1) {
	//				SetDirAndOnePass(d);
	//			} else if (canMoveLeft 		&& selection == 2) {
	//				SetDirAndOnePass(l);
	//			} else if (canMoveRight 	&& selection == 3) {
	//				SetDirAndOnePass(r);
	//			} else {
	//				RandomMovement();
	//			}
	//		}
	//	}
	//}
	//}

	bool madeCorrectChoice = false;

	void RandomMovement() {
		Debug.Log(Time.deltaTime);
		if (atIntersection == true) {
			if (posDirInputWasMade != enemyManager.PlayerPosition) {
				madeCorrectChoice = false;
				int c;
				c = Random.Range(0, 4);

				switch (c) {
					case 0:
						if (canMoveUp == true) {
							madeCorrectChoice = true;
							SetDirAndOnePass(u);
						}
						break;
					case 1:
						if (canMoveDown == true) {
							madeCorrectChoice = true;
							SetDirAndOnePass(d);
						}
						break;
					case 2:
						if (canMoveLeft == true) {
							madeCorrectChoice = true;
							SetDirAndOnePass(l);
						}
						break;
					case 3:
						if (canMoveRight == true) {
							madeCorrectChoice = true;
							SetDirAndOnePass(r);
						}
						break;
					default:
						break;
				}

				if (madeCorrectChoice == false) {
					RandomMovement();
				}
			}
		}
	}



	bool gtp_flipFlop = false;

	void GoToPosition() {
		if (atIntersection == true) {
			if (posDirInputWasMade != enemyManager.PlayerPosition) {
				if (gtp_flipFlop == true) {
					if (enemyManager.PlayerPosition.y < randomlySelectedPoint.y) {
						if (canMoveDown) {
							SetDirAndOnePass(d);
						}
					} else {
						if (canMoveUp) {
							SetDirAndOnePass(u);
						}
					}
				} else {
					if (enemyManager.PlayerPosition.x < randomlySelectedPoint.x) {
						if (canMoveRight) {
							SetDirAndOnePass(r);
						}
					} else {
						if (canMoveLeft) {
							SetDirAndOnePass(l);
						}
					}
				}
				gtp_flipFlop = !gtp_flipFlop;
			}
		}
	}




	[SerializeField]
	bool canMoveDown, canMoveUp, canMoveLeft, canMoveRight;
	bool atIntersection;
	int amountOfIntersections;

	void CheckWhenAtIntersection() { // TODO MAKE DYNAMIC
		amountOfIntersections = 0;
		eom_dir = ConvertArrToVec();

		if (eom_dir == Vector2Int.up) {
			ResetDirectionValues();
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y - 1].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y - 1].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveUp = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x + 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x + 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveRight = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x - 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x - 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveLeft = true;
			}
			if (amountOfIntersections > 1) {
				atIntersection = true;
			} else {
				atIntersection = false;
			}
			canMoveDown = false;
		}
		if (eom_dir == Vector2Int.down) {
			ResetDirectionValues();
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y + 1].GetComponent<ChangeColour>().startCol == 1 ||
					StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y + 1].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveDown = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x + 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x + 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveRight = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x - 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x - 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveLeft = true;
			}
			if (amountOfIntersections > 1) {
				atIntersection = true;
			} else {
				atIntersection = false;
			}
			canMoveUp = false;
		}
		if (eom_dir == Vector2Int.left) {
			ResetDirectionValues();
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x - 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x - 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveLeft = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y + 1].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y + 1].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveDown = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y - 1].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y - 1].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveUp = true;
			}
			if (amountOfIntersections > 1) {
				atIntersection = true;
			} else {
				atIntersection = false;
			}
			canMoveRight = false;
		}
		if (eom_dir == Vector2Int.right) {
			ResetDirectionValues();
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x + 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x + 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveRight = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y + 1].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y + 1].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveDown = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y - 1].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y - 1].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveUp = true;
			}
			if (amountOfIntersections > 1) {
				atIntersection = true;
			} else {
				atIntersection = false;
			}
			canMoveLeft = false;
		}
		if (eom_dir == Vector2Int.zero) {
			ResetDirectionValues();
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x + 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 1 ||
					StaticData.GameObjectList[enemyManager.PlayerPosition.x + 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveRight = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x - 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x - 1, enemyManager.PlayerPosition.y].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveLeft = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y + 1].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y + 1].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveDown = true;
			}
			if (StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y - 1].GetComponent<ChangeColour>().startCol == 1 ||
				 	StaticData.GameObjectList[enemyManager.PlayerPosition.x, enemyManager.PlayerPosition.y - 1].GetComponent<ChangeColour>().startCol == 2) {
				amountOfIntersections++;
				canMoveUp = true;
			}
			if (amountOfIntersections > 0) {
				atIntersection = true;
			} else {
				atIntersection = false;
			}
			state = startState;
		}
	}

	void ResetDirectionValues() {
		canMoveUp = false;
		canMoveDown = false;
		canMoveLeft = false;
		canMoveRight = false;
	}

	Vector2Int ConvertArrToVec() {
		Vector2Int ret;
		ret = new Vector2Int(enemyManager.PlayerDir[0], -enemyManager.PlayerDir[1]);
		return ret;
	}
}
