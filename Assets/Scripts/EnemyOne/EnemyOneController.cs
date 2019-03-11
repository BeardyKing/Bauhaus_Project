﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOneController : MonoBehaviour
{
	// ENEMY COLOUR = RED (2)
	EnemyOneManager enemyManager;
	Vector2 input;

	[Range(.1f, Mathf.Infinity)]
	public float bufferTimer;

	void Start()
    {
		enemyManager = GetComponent<EnemyOneManager>();

	}

	void Update()
    {
		//input = Vector2.right;
		input = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical")
			).normalized;

		enemyManager.PushInput(input, bufferTimer);
	}
}
