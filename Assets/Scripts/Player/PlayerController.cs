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
				}
			}
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

	void SetDirAndOnePass(Vector2Int inDir) { // TODO

		directionsMade++;
		if (directionsMade > inputsNeededToGrow) {
			directionsMade = 0;
			if (pm.MaxLength < lengthCap) {

				pm.MaxLength++;
				Debug.Log("DOING INPUT");
				canAdd = false;
			}
		}
	}
}








