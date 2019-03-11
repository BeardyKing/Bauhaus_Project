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

		}
	}






