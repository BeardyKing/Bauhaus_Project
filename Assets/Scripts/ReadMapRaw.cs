using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

using UnityEngine;

public class ReadMapRaw : MonoBehaviour{


	public int[,] map;

	string path = @"Assets / map.txt";

	public int x;
	public int y;
	bool startCount = false;

	void Awake() {
		//x = 0;
		//y = 0;
	}
	bool singlePass = true;
	bool contentPass = true;
	bool checkX = true;

	int c = 0;
	void Start() {

	}
	void FindSizeMap() {
		foreach (var item in StaticData.RawMap) {
			foreach (char content in StaticData.RawMap[c]) {
				if (content == '(') {
					startCount = true;
				}if (content == ')') {
					startCount = false;
				}
			}
			if (startCount == true) {
				if (contentPass == false) {
					if (checkX == true) {
						checkX = false;
						x = StaticData.RawMap[c].Length;
					}
					y++;
				}
				contentPass = false;
			}
			c++;
		}
		map = new int[y , x];
	}

	int temp;
	int i2 = 0;
	int y2 = 0;

	void FillMapWithData() {
		for (int i = 2; i < y + 2; i++) {
			foreach (var item in StaticData.RawMap[i]) {
				temp = (int)Char.GetNumericValue(item);
				map[y2, i2] = temp;
				if (temp == 4) {
					StaticData.PlayerPos[0] = y2;
					StaticData.PlayerPos[1] = i2;
				}
				i2++;
			}
			//Debug.Log("______" + i);
			i2 = 0;
			y2++;
		}
		StaticData.Map = map;
	}

	void Update() {
		if (singlePass == true) {
			singlePass = false;
			FindSizeMap();
			FillMapWithData();
		}
	}
}
