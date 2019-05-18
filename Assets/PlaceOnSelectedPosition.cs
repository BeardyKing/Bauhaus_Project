using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOnSelectedPosition : MonoBehaviour
{
	// Start is called before the first frame update
	[SerializeField]
	public enum Col {red, blue, yellow}
	public Col colourSelected;

	Vector3 targetPos;
	float zoffset = -.4f;
	PlayerManager pm;
	EnemyOneManager em1;
	EnemyTwoManager em2;
    void Start(){
		pm = FindObjectOfType<PlayerManager>();
		em1 = FindObjectOfType<EnemyOneManager>();
		em2 = FindObjectOfType<EnemyTwoManager>();
    }

    // Update is called once per frame
    void Update(){
		if (StaticData.GameRunning == true) {
			if (colourSelected == Col.yellow) 
				transform.position = new 	Vector3(em2.PlayerPosition.x, 	-em2.PlayerPosition.y, zoffset);
			if (colourSelected == Col.blue) 
				transform.position = new 	Vector3(pm.PlayerPosition.x, 	-pm.PlayerPosition.y, zoffset);
			if (colourSelected == Col.red) 
				transform.position = new 	Vector3(em1.PlayerPosition.x, 	-em1.PlayerPosition.y, zoffset);
		} else {
			Destroy(this.gameObject);
		}
	}
}
