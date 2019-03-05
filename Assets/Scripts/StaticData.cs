using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StaticData{
	private static GameObject[,] gameObjectList;
	private static bool gameRunning = false;
	private static int[] playerPos = new int[2];
	private static string mapPath;
	private static string[] rawMap;
	private static int[,] map;

	public static string MapPath {
		get {
			return MapPath;
		}
		set {
			mapPath = value;
		}
	}

	public static GameObject[,] GameObjectList {
		get {
			return gameObjectList;
		}
		set {
			gameObjectList = value;
		}
	}

	public static bool GameRunning {
		get {
			return GameRunning;
		}
		set {
			gameRunning = value;
		}
	}

	public static string[] RawMap {
		set {
			rawMap = value;
		}
		get {
			return rawMap;
		}
	}
	public static int[,] Map {
		get {
			return map;
		}
		set {
			map = value;
		}
	}
	private static int num = 0;

	public static int Number {
		get {
			return num;
		}
		set {
			num = value;
		}
	}
	public static int[] PlayerPos {
		get {
			return playerPos;
		}
		set {
			playerPos = value;
		}
	}
}
