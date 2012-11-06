using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public static class LevelLoader {

	public static void LoadLevel(string filename) {
		StreamReader reader;
		try {
			reader = new StreamReader(filename);
		}
		catch(Exception) {
			Debug.Log("Shit, bro! This file didn't work. Filename was: " + filename);
			return;
		}
		int width = Int32.Parse(reader.ReadLine());
		int height = Int32.Parse(reader.ReadLine());
		Grid grid = new Grid(width, height);
		string line;
		while((line = reader.ReadLine()) != null) {
			String[] parts = line.Split(new char[] {' '});
			int x = Int32.Parse(parts[0]);
			int y = Int32.Parse(parts[1]);
			for(int i = 2; i < parts.Length;) {
				int type = Int32.Parse(parts[i++]);
				switch(type) {
					case 0: // Wall
						grid.Add(ParseWall(x, y, CopyRange(parts, i, 3)), x, y);
						i += 4;
						break;
					case 1: // SpikeWall
						grid.Add(ParseSpikeWall(x, y, CopyRange(parts, i, 4)), x, y);
						i += 5;
						break;
					case 2: // SpikeFloor
						grid.Add(SpikeFloor.MakeSpikeFloor(x, y), x, y);
						i += 2;
						break;
					case 3: // Paint
						grid.Add(ParsePaint(x, y, CopyRange(parts, i, 3)), x, y);
						i += 4;
						break;
					case 4: // Conveyor
						ParseConveyor(x, y, CopyRange(parts, i, 5));
						i += 6;
						break;
					case 5: // Player
						grid.Add(ParsePlayer(x, y, CopyRange(parts, i, 1)), x, y);
						i += 2;
						break;
					case 6: // Robot
						grid.Add(ParseRobot(x, y, CopyRange(parts, i, 9)), x, y);
						i += 10;
						break;
				}
			}
		}
		GameManager.floor = grid;
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
				return default(Color);
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
	
	public static GameObject ParseWall(int x, int y, string[] info) {
		int health = Int32.Parse(info[0]);
		bool destructible = Int32.Parse(info[1]) == 1 ? true : false;
		Color color = ParseColor(info[2]);
		return Wall.MakeWall(x, y, health, destructible, color);
	}

	public static GameObject ParseSpikeWall(int x, int y, string[] info) {
		int health = Int32.Parse(info[0]);
		bool destructible = Int32.Parse(info[1]) == 1 ? true : false;
		List<Vector2> directions = new List<Vector2>();
		int i = 3;
		for(; info[i] != "]"; i++) {
			directions.Add(ParseVector2(info[i]));
		}
		Color color = ParseColor(info[++i]);
		return SpikeWall.MakeSpikeWall(x, y, health, destructible, directions, color);
	}
	
	public static GameObject ParsePaint(int x, int y, string[] info) {
		Color color = ParseColor(info[0]);
		float respawnTime = Single.Parse(info[1]);
		return Paint.MakePaint(x, y, color, respawnTime);
	}

	public static void ParseConveyor(int x, int y, string[] info) {
		Vector2 direction = ParseVector2(info[0]);
		float length = Single.Parse(info[1]);
		float speed = Single.Parse(info[2]);
		bool switchable = Int32.Parse(info[3]) == 1 ? true : false;
		float switchRate = Single.Parse(info[4]);
		Conveyor.MakeConveyor(new Vector2(x, y), direction, length, speed, switchable, switchRate);
	}
	
	public static GameObject ParsePlayer(int x, int y, string[] info) {
		int health = Int32.Parse(info[0]);
		return Player.MakePlayer(x, y, health);
	}

	public static GameObject ParseRobot(int x, int y, string[] info) {
		float speed = Single.Parse(info[0]);
		int damageDealt = Int32.Parse(info[1]);
		int health = Int32.Parse(info[2]);
		int forwardRange = Int32.Parse(info[3]);
		int sideRange = Int32.Parse(info[4]);
		Vector2 movementDirection = ParseVector2(info[5]);
		Color colorVisible = ParseColor(info[6]);
		Vector2 fireDirection = ParseVector2(info[7]);
		bool turnsLeft = Int32.Parse(info[8]) == 1 ? true : false;
		return Robot.MakeRobot(x, y, speed, damageDealt, health, forwardRange, sideRange,
				       movementDirection, colorVisible, fireDirection, turnsLeft);
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