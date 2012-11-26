using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class LevelLoader {
	
	public static Grid grid;
	
	public static Grid LoadLevel(string filename, out string audiofile) {
		audiofile = "";
		StreamReader reader;
		string path = "";
		if(Application.isEditor)
			path = Path.Combine(Application.dataPath + "/Resources/Levels", filename);
		else
			path = Path.Combine(Application.dataPath, filename);
		try {
			reader = new StreamReader(path);
		}
		catch(Exception) {
			Debug.Log("Shit, bro! This file didn't work. Filename was: " + filename);
			return null;
		}
		Debug.Log("Loaded from " + path);
		int width = Int32.Parse(reader.ReadLine());
		int height = Int32.Parse(reader.ReadLine());
		grid = new Grid(width, height);
		WinChecker.robotsWin = false;
		WinChecker.squareWins = false;
		WinChecker.robotLimit = -1;
		WinChecker.numRobots = 0;
		WinChecker.winCoords = default(Vector2);
		string line = reader.ReadLine();
		string[] conditions = line.Split(new char[] {' '});
		for(int i = 0; i < conditions.Length; i++) {
			int type = Int32.Parse(conditions[i++]);
			switch(type) {
				case 0: // Robot
					LevelEditor.robotLimit = conditions[i];
					LevelEditor.robotsWin = true;
					WinChecker.robotsWin = true;
					WinChecker.robotLimit = Int32.Parse(conditions[i++]);
					break;
				case 1: // Wall
					WinChecker.squareWins = true;
					Vector2 winCoords;
					winCoords.x = Int32.Parse(conditions[i++]);
					winCoords.y = Int32.Parse(conditions[i++]);
					WinChecker.winCoords = winCoords;
					LevelEditor.squareWins = true;
					LevelEditor.winCoords = winCoords.x + ", " + winCoords.y;
					grid.grid[(int)winCoords.x, (int)winCoords.y].plane.renderer.material.mainTexture = Resources.Load("Textures/Tile.png") as Texture;
					grid.grid[(int)winCoords.x, (int)winCoords.y].plane.name = "Win Square";
					break;
			}
		}
		while((line = reader.ReadLine()) != null) {
			if(line.StartsWith("#"))
				continue;
			string[] parts = line.Split(new char[] {' '});
			int x = Int32.Parse(parts[0]);
			int y = Int32.Parse(parts[1]);
			for(int i = 2; i < parts.Length;) {
				int type = Int32.Parse(parts[i++]);
				switch(type) {
					case 0: // Wall
						grid.Add(Wall.MakeWall(grid, x, y), x, y);
						break;
					case 1: // SpikeWall
						grid.Add(ParseSpikeWall(x, y, CopyRange(parts, i, 5)), x, y);
						i += 5;
						break;
					case 2: // SpikeFloor
						grid.Add(SpikeFloor.MakeSpikeFloor(grid, x, y), x, y);
						i += 1;
						break;
					case 3: // Paint
						grid.Add(ParsePaint(x, y, CopyRange(parts, i, 2)), x, y);
						i += 2;
						break;
					case 4: // Conveyor
						ParseConveyor(x, y, CopyRange(parts, i, 5));
						i += 5;
						break;
					case 5: // Player
						GameObject player = ParsePlayer(x, y, CopyRange(parts, i, 1));
						grid.Add(player, x, y);
						GameManager.player = player.GetComponent<Player>();
						i += 1;
						break;
					case 6: // Robot
						grid.Add(ParseRobot(x, y, CopyRange(parts, i, 11)), x, y);
						i += 11;
						break;
					case 7: // DestructibleWall
						grid.Add(ParseDestructibleWall(x, y, CopyRange(parts, i, 2)), x, y);
						i += 2;
						break;
					case 8: // ExplosiveCrate
						grid.Add(ParseExplosiveCrate(x, y, CopyRange(parts, i, 3)), x, y);
						i += 3;
						break;
					case 9: // Audio file
						audiofile = ParseAudioFile(parts, i);
						i = parts.Length;
						break;
				}
			}
		}
		reader.Close();
		return grid;
	}
	
	/*
	 * Takes a string and returns the corresponding color.
	 * 0 is red, 1 is green, 2 is blue, 3 is the default color.
	 */
	public static Color ParseColor(string colorString) {
		int val = Int32.Parse(colorString);
		switch(val) {
			case 0:
				return Color.red;
			case 1:
				return Color.green;
			case 2:
				return Color.blue;
			default:
				return Color.white;
		}
	}
	
	public static Vector2 ParseVector2(string vectorString) {
		int val = Int32.Parse(vectorString);
		switch(val) {
			case 0:
				return new Vector2(0, 1);
			case 1:
				return new Vector2(1, 0);
			case 2:
				return new Vector2(0, -1);
			case 3:
				return new Vector2(-1, 0);
			default:
				return default(Vector2);
		}
	}
	
	public static RotationMatrix ParseRotationMatrix(string rotationString) {
		int val = Int32.Parse(rotationString);
		switch(val) {
			case 0:
				return new RotationMatrix(RotationMatrix.Rotation.Identity);
			case 1:
				return new RotationMatrix(RotationMatrix.Rotation.Left);
			case 2:
				return new RotationMatrix(RotationMatrix.Rotation.Right);
			default:
				return new RotationMatrix(RotationMatrix.Rotation.Halfway);
		}
	}

	public static GameObject ParseSpikeWall(int x, int y, string[] info) {
		List<Vector2> directions = new List<Vector2>();
		int i = 3;
		for(; info[i] != "]"; i++) {
			directions.Add(ParseVector2(info[i]));
		}
		Color color = ParseColor(info[++i]);
		return SpikeWall.MakeSpikeWall(grid, x, y, directions, color);
	}
	
	public static GameObject ParsePaint(int x, int y, string[] info) {
		Color color = ParseColor(info[0]);
		float respawnTime = Single.Parse(info[1]);
		return Paint.MakePaint(grid, x, y, color, respawnTime);
	}

	public static void ParseConveyor(int x, int y, string[] info) {
		Vector2 direction = ParseVector2(info[0]);
		float length = Single.Parse(info[1]);
		float speed = Single.Parse(info[2]);
		bool switchable = Int32.Parse(info[3]) == 1 ? true : false;
		float switchRate = Single.Parse(info[4]);
		Conveyor.MakeConveyor(grid, new Vector2(x, y), direction, length, speed, switchable, switchRate);
	}
	
	public static GameObject ParsePlayer(int x, int y, string[] info) {
		int health = Int32.Parse(info[0]);
		return Player.MakePlayer(grid, x, y, health);
	}

	public static GameObject ParseRobot(int x, int y, string[] info) {
		float speed = Single.Parse(info[0]);
		float fireRate = Single.Parse(info[1]);
		int damageDealt = Int32.Parse(info[2]);
		int health = Int32.Parse(info[3]);
		int forwardRange = Int32.Parse(info[4]);
		int sideRange = Int32.Parse(info[5]);
		Vector2 movementDirection = ParseVector2(info[6]);
		Color colorVisible = ParseColor(info[7]);
		Color colorPainted = ParseColor(info[8]);
		Vector2 fireDirection = ParseVector2(info[9]);
		RotationMatrix rotation = ParseRotationMatrix(info[10]);
		return Robot.MakeRobot(grid, x, y, speed, fireRate, damageDealt, health, forwardRange, sideRange,
				       movementDirection, colorVisible, colorPainted, fireDirection, rotation);
	}

	public static GameObject ParseDestructibleWall(int x, int y, string[] info) {
		int health = Int32.Parse(info[0]);
		Color color = ParseColor(info[1]);
		return DestructibleWall.MakeDestructibleWall(grid, x, y, health, color);
	}
	
	public static GameObject ParseExplosiveCrate(int x, int y, string[] info) {
		int health = Int32.Parse(info[0]);
		int range = Int32.Parse(info[1]);
		int damageDealt = Int32.Parse(info[2]);
		return ExplosiveCrate.MakeExplosiveCrate(grid, x, y, health, range, damageDealt);
	}
	
	public static string ParseAudioFile(string[] info, int i) {
		StringBuilder sb = new StringBuilder();
		for(; i < info.Length; i++) {
			sb.Append(info[i] + " ");
		}
		Debug.Log(sb.ToString());
		return sb.ToString().Trim();
	}
	
	/*
	 * This just copies a section of an array. It's useful when passing parameters
	 * to one of the parsing functions.
	 */
	public static string[] CopyRange(string[] array, int start, int count) {
		string[] copied = new string[count];
		for(int i = 0; i < count; i++) {
			copied[i] = array[i + start];
		}
		return copied;
	}
}
