using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CountBlockTypes : MonoBehaviour{
	public TextMesh[] colText; // W/R/B/Y
	public TextMesh txt_winner;
	[SerializeField]
	public int[] colAmount = {0,0,0,0};
	int counter = 7;
    void Start(){

	}

	void Update() {
		counter--;
		if (counter <= 0) {
		colAmount = new int[] { 0, 0, 0, 0 };

			for (int i = 0; i < 40; i++) {
				for (int j = 0; j < 29; j++) {
					if (StaticData.GameObjectList[i, j].gameObject.GetComponent<ChangeColour>().startCol == 0) {
						colAmount[0]++;
					} else if (StaticData.GameObjectList[i, j].gameObject.GetComponent<ChangeColour>().startCol == 5) {
						colAmount[1]++;
					} else if (StaticData.GameObjectList[i, j].gameObject.GetComponent<ChangeColour>().startCol == 6) {
						colAmount[3]++;
					} else if (StaticData.GameObjectList[i, j].gameObject.GetComponent<ChangeColour>().startCol == 7) {
						colAmount[2]++;
					}
				}
			}
			// 140 edge blocks
			colAmount[0] -= 140;
			colText[0].text = colAmount[0].ToString();
			colText[1].text = colAmount[1].ToString();
			colText[2].text = colAmount[2].ToString();
			colText[3].text = colAmount[3].ToString();
			counter = 25;

			if (colAmount[0] <= 240) { // 250 took 6 minutes 17 of the beta map
				// GAME OVER
				if (colAmount[1] > colAmount[2] && colAmount[1] > colAmount[3]) {
					txt_winner.text = "RED WINS";
					StaticData.WinningColour = 1;
				}
				if (colAmount[2] > colAmount[1] && colAmount[2] > colAmount[3]) {
					txt_winner.text = "BLUE WINS";
					StaticData.WinningColour = 2;
				}
				if (colAmount[3] > colAmount[1] && colAmount[3] > colAmount[2]) {
					txt_winner.text = "YELLOW WINS";
					StaticData.WinningColour = 3;
				}
				StaticData.GameRunning = false;
				HidePlayerBody();
			}
		}
	}

	bool singlePass;

	void HidePlayerBody() {
		for (int i = 0; i < 40; i++) {
			for (int j = 0; j < 29; j++) {
				if (StaticData.GameObjectList[i, j].gameObject.GetComponent<ChangeColour>().startCol == 2||
					StaticData.GameObjectList[i, j].gameObject.GetComponent<ChangeColour>().startCol == 3||
					StaticData.GameObjectList[i, j].gameObject.GetComponent<ChangeColour>().startCol == 4) 
					{
					StaticData.GameObjectList[i, j].gameObject.GetComponent<ChangeColour>().startCol = 1;
					}
			}
		}
	}
}
