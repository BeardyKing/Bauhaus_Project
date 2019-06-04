using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CountBlockTypes : MonoBehaviour{
	public TextMesh[] colText; // W/R/B/Y
	public TextMeshPro txt_winner;
	[SerializeField]
	public int[] colAmount = {0,0,0,0};
	int counter = 7;
	public bool rWin = false, bWin = false, yWin = false;
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

			if (colAmount[0] <= 300 || StaticData.GameRunning == false) { // 250 took 6 minutes 17 of the beta map
				// GAME OVER
				if (colAmount[1] > colAmount[2] && colAmount[1] > colAmount[3]) {
					txt_winner.text = "\"" + "THE VICTORY OF BLUE" + "\"";
					StaticData.WinningColour = 1;
					rWin = true;
					bWin = false;
					yWin = false;
				}
				else if (colAmount[3] > colAmount[1] && colAmount[3] > colAmount[2])
                {
                    txt_winner.text = "\"" + "THE VICTORY OF YELLOW" + "\"";
                    StaticData.WinningColour = 3;
					yWin = true;
					rWin = false;
					bWin = false;
				}
				//if (colAmount[2] > colAmount[1] && colAmount[2] > colAmount[3]) {
				else
                {
                    txt_winner.text = "\"" + "THE VICTORY OF RED" + "\"";
                    StaticData.WinningColour = 2;
					bWin = true;
					rWin = false;
					yWin = false;
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
