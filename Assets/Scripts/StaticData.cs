using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StaticData {
	private static GameObject[,] gameObjectList;
	private static int[] playerPos = {0,0};
	private static string[] rawMap;
	private static int[,] map;
	private static int[] enemyOnePos = { 0, 0 };
	private static int[] enemyTwoPos = { 0, 0 };


	#region waiting for implimentation
	//private static bool gameRunning = false; // TODO MAKE THIS THE MANAGER FOR ALL MAJOR SCRIPTS
	//public static bool GameRunning {
	//	get {
	//		return GameRunning;
	//	}
	//	set {
	//		gameRunning = value;
	//	}
	//}

	//private static string mapPath; // DEPRICATED 
	//public static string MapPath {
	//	get {
	//		return MapPath;
	//	}
	//	set {
	//		mapPath = value;
	//	}
	//}
	#endregion

	public static GameObject[,] GameObjectList {
		get {
			return gameObjectList;
		}
		set {
			gameObjectList = value;
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
	//public static Vector2Int PlayerPos {
	//	get {
	//		return playerPos;
	//	}
	//	set {
	//		playerPos = value;
	//	}
	//}
	public static int[] EnemyOnePos {
		get {
			return enemyOnePos;
		}
		set {
			enemyOnePos = value;
		}
	}

	public static int[] EnemyTwoPos {
		get {
			return enemyTwoPos;
		}
		set {
			enemyTwoPos = value;
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
