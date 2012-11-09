using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class LevelWriter {

	public static void WriteLevel(string filename, Grid grid) {
		string path = Path.Combine(Application.persistentDataPath, filename);
		using (StreamWriter writer = File.CreateText(filename)) {
			writer.WriteLine(grid.width);
			writer.WriteLine(grid.height);
			for(int i = 0; i < grid.width; i++) {
				for(int j = 0; j < grid.height; j++) {
					StringBuilder sb = new StringBuilder();
					sb.Append(i + " " + j + " "); // append the x and y coordinates of the grid Square in question
					Square sq = grid.grid[i, j];

					// if the Square contains a Wall, encode the relevant information
					GameObject obj = sq.objects.Find((GameObject g) => g.GetComponent<Wall>() != null);
					if(obj != null) {
						Wall wall = obj.GetComponent<Wall>();
						if(wall != null) {
							sb.Append(0 + " " + wall.health + " ");
							
							if(wall.destructible == false)
								sb.Append(0 + " ");
							else
								sb.Append(1 + " ");
							
							if(wall.colorPainted == Color.red)
								sb.Append(0 + " ");
							else if(wall.colorPainted == Color.green)
								sb.Append(1 + " ");
							else if(wall.colorPainted == Color.blue)
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
						}
					}
					// if the Square contains a SpikeWall, encode the relevant information
					obj = sq.objects.Find((GameObject g) => g.GetComponent<SpikeWall>() != null);
					if(obj != null) {
						SpikeWall spike = obj.GetComponent<SpikeWall>();
						if(spike != null) {
							sb.Append(1 + " " + spike.health + " ");
							
							if(spike.destructible == false)
								sb.Append(0 + " ");
							else
								sb.Append(1 + " ");
							
							sb.Append("[ ");
							foreach(Vector2 dir in spike.directions) {
								if(dir == new Vector2(0, 1))
									sb.Append(0 + " ");
								else if(dir == new Vector2(1, 0))
									sb.Append(1 + " ");
								else if(dir == new Vector2(0, -1))
									sb.Append(2 + " ");
								else if(dir == new Vector2(-1, 0))
									sb.Append(3 + " ");
							}
							sb.Append("] ");
							
							if(spike.colorPainted == Color.red)
								sb.Append(0 + " ");
							else if(spike.colorPainted == Color.green)
								sb.Append(1 + " ");
							else if(spike.colorPainted == Color.blue)
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
						}
					}
					// if the Square contains a SpikeFloor, encode the relevant information
					obj = sq.objects.Find((GameObject g) => g.GetComponent<SpikeFloor>() != null);
					if(obj != null) {
						SpikeFloor spikefloor = obj.GetComponent<SpikeFloor>();
						if(spikefloor != null)
							sb.Append(2 + " ");
					}
					// if the Square contains a Paint, encode the relevant information
					obj = sq.objects.Find((GameObject g) => g.GetComponent<Paint>() != null);
					if(obj != null) {
						Paint paint = obj.GetComponent<Paint>();
						if(paint != null) {
							sb.Append(3 + " ");
							
							if(paint.colorPainted == Color.red)
								sb.Append(0 + " ");
							else if(paint.colorPainted == Color.green)
								sb.Append(1 + " ");
							else if(paint.colorPainted == Color.blue)
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
							
							sb.Append(paint.respawnTime + " ");
						}
					}
					// if the Square contains a Conveyor, encode the relevant information
					obj = sq.objects.Find((GameObject g) => g.GetComponent<Conveyor>() != null);
					if(obj != null) {
						Conveyor conveyor = obj.GetComponent<Conveyor>();
						if(conveyor != null) {
							sb.Append(4 + " ");

							if(conveyor.direction == new Vector2(0, 1))
								sb.Append(0 + " ");
							else if(conveyor.direction == new Vector2(1, 0))
								sb.Append(1 + " ");
							else if(conveyor.direction == new Vector2(0, -1))
								sb.Append(2 + " ");
							else if(conveyor.direction == new Vector2(-1, 0))
								sb.Append(3 + " ");

							sb.Append(conveyor.length + " " + conveyor.speed + " ");

							if(conveyor.switchable == false) {
								sb.Append(0 + " "); // switchable == false
								sb.Append(0 + " "); // switchRate == 0							
							}
							else {
								sb.Append(1 + " ");
								sb.Append(conveyor.switchRate + " ");
							}
						}
					}
					// if the Square contains a Player, encode the relevant information
					obj = sq.objects.Find((GameObject g) => g.GetComponent<Player>() != null);
					if(obj != null) {
						Player player = obj.GetComponent<Player>();
						if(player != null)
							sb.Append(5 + " " + player.health + " ");
					}
					// if the Square contains a Robot, encode the relevant information
					obj = sq.objects.Find((GameObject g) => g.GetComponent<Robot>() != null);
					if(obj != null) {
						Robot robot = obj.GetComponent<Robot>();
						if(robot != null) {
							sb.Append(6 + " " + robot.moveSpeed + " " + robot.damageDealt + " " + robot.health + " " + robot.forwardRange + " " + robot.sideRange + " ");
							
							if(robot.movementDirection == new Vector2(0, 1))
								sb.Append(0 + " ");
							else if(robot.movementDirection == new Vector2(1, 0))
								sb.Append(1 + " ");
							else if(robot.movementDirection == new Vector2(0, -1))
								sb.Append(2 + " ");
							else if(robot.movementDirection == new Vector2(-1, 0))
								sb.Append(3 + " ");
							
							if(robot.colorVisible == Color.red)
								sb.Append(0 + " ");
							else if(robot.colorVisible == Color.green)
								sb.Append(1 + " ");
							else if(robot.colorVisible == Color.blue)
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
							
							if(robot.fireDirection == new Vector2(0, 1))
								sb.Append(0 + " ");
							else if(robot.fireDirection == new Vector2(1, 0))
								sb.Append(1 + " ");
							else if(robot.fireDirection == new Vector2(0, -1))
								sb.Append(2 + " ");
							else if(robot.fireDirection == new Vector2(-1, 0))
								sb.Append(3 + " ");
							
							if(robot.turnsLeft == false)
								sb.Append(0 + " ");
							else
								sb.Append(1 + " ");
						}
					}
					string line = sb.ToString().Trim();
					// Don't just write x and y coords if we don't need them
					if(line.Split(new char[] {' '}).Length > 2) {
						writer.WriteLine(line); // write all this crap to a line in the text file
					}
				}
			}
			writer.Close();
			Debug.Log("Saved to " + path);
		}
	/*	else
			Debug.Log("Dude, what are you trying to do here? Filename: " + filename + " already exists. Just stop already.");*/
	}
}
