using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class LevelWriter {
	
	public static void WriteLevel(string filename) {
		string path = Path.Combine(Application.persistentDataPath, filename);

		if(!File.Exists(path)) { //take this line out if we want to overwrite files
			using (StreamWriter writer = File.CreateText(path)) {
				sb.WriteLine(GameManager.floor.width);
				sb.WriteLine(GameManager.floor.height);
				for(int i = 0; i < GameManager.floor.width; i++) {
					for(int j = 0; j < GameManager.floor.height; j++) {
						StringBuilder sb = new StringBuilder();
						sb.Append(i + " " + j + " "); // append the x and y coordinates of the grid Square in question
						Square sq = GameManager.floor.grid[i, j];

						// if the Square contains a Wall, encode the relevant information
						Wall wall = sq.objects.Find((GameObject g) => g.GetComponent<Wall>() != null).GetComponent<Wall>();
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

						// if the Square contains a SpikeWall, encode the relevant information
						SpikeWall spike = sq.objects.Find((GameObject g) => g.GetComponent<SpikeWall>() != null).GetComponent<SpikeWall>();
						if(spike != null) {
							sb.Append(1 + " " + spike.health + " ");

							if(wall.destructible == false)
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

						// if the Square contains a SpikeFloor, encode the relevant information
						SpikeFloor spikefloor = sq.objects.Find((GameObject g) => g.GetComponent<SpikeFloor>() != null).GetComponent<SpikeFloor>();
						if(spikefloor != null)
							sb.Append(2 + " ");

						// if the Square contains a Paint, encode the relevant information
						Paint paint = sq.objects.Find((GameObject g) => g.GetComponent<Paint>() != null).GetComponent<Paint>();
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

						// if the Square contains a Conveyor, encode the relevant information
						Conveyor conveyor = sq.objects.Find((GameObject g) => g.GetComponent<Conveyor>() != null).GetComponent<Conveyor>();
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

						// if the Square contains a Player, encode the relevant information
						Player player = sq.objects.Find((GameObject g) => g.GetComponent<Player>() != null).GetComponent<Player>();
						if(player != null)
							sb.Append(5 + " " + player.health + " ");

						// if the Square contains a Robot, encode the relevant information
						Robot robot = sq.objects.Find((GameObject g) => g.GetComponent<Player>() != null).GetComponent<Robot>();
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

						writer.WriteLine(sb.ToString()); // write all this crap to a line in the text file
					}
				}
			}
		}
		else
			Debug.Log("Dude, what are you trying to do here? Filename: " + filename + " already exists. Just stop already.");
	}
}
