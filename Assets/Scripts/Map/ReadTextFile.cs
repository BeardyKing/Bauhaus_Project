﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
public class ReadTextFile : MonoBehaviour
{
	//[SerializeField]
	//public TextAsset Text;
	//string path = @"Assets/map.txt";
	string path;
	int arrSizeX = 40;
	int arrSizeY = 30;
	string appendText = "";
	public string[] rawText;




    public TextMesh boi;

	void Awake()
    {
        //Debug.Log(Application.persistentDataPath);
		if (Application.isEditor == true) {
			path = Application.dataPath + "/StreamingAssets/map.txt";
		} else {
            //if (Application.platform == RuntimePlatform.WindowsEditor) {
            //    path = Application.dataPath + "/Snake_Data/StreamingAssets/map.txt";
            //}
            path = Application.streamingAssetsPath + "/map.txt"; //"/Resources/Data/StreamingAssets/map.txt";

            //path = Application.dataPath + "/Resources/Data/StreamingAssets/map.txt";
		}
		if (File.Exists(path)) {
			boi.text = "TRUE   " + path;
		} else {
			boi.text = "FALSE   " + path;
		}

		//StaticData.MapPath = path;
		//CreateTextFile();
		//ContentsTextFile();
		//EndTextFile();
		ReadText();
	}

	void CreateTextFile() {
		string createText;
		//if (!File.Exists(path)) {
		createText = "[size:" + 
			arrSizeX + "," + 
			arrSizeY + "]" + 
			Environment.NewLine + "(" + 
			Environment.NewLine;

		File.WriteAllText(path, createText);
		//}
	}

	void ContentsTextFile() {
		for (int i = 0; i < arrSizeY; i++) {
			for (int j = 0; j < arrSizeX; j++) {
				appendText +=  0;
			}
			appendText += "" + Environment.NewLine;
		}
		File.AppendAllText(path, appendText);
	}

	void EndTextFile() {
		appendText = "";
		appendText += ")" + Environment.NewLine;
		File.AppendAllText(path, appendText);

	}

	void ReadText2() {
		FileStream stream = new FileStream(path, FileMode.Open);

	

	}

	void ReadText() {
		rawText = File.ReadAllLines(path);
		StaticData.RawMap = rawText;
	}
}
