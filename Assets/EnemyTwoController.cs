using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoController : MonoBehaviour {
	// ENEMY COLOUR = RED (2)
	EnemyTwoManager enemyManager;
	Vector2 input;

	[Range(.001f, 1f)]
	public float bufferTimer;

	void Start() {
		enemyManager = GetComponent<EnemyTwoManager>();

	}

	void Update() {
		//input = Vector2.right;
		input = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical")
			).normalized;

		//enemyManager.PushInput(input, bufferTimer);
	}
}
