using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class LevelWriter {

	public static void WriteLevel(string filename, Grid grid) {
		string path = "";
		if(Application.isEditor)
			path = Path.Combine(Application.dataPath + "/Resources/Levels", filename);
		else
			path = Path.Combine(Application.dataPath, filename);
		using (StreamWriter writer = File.CreateText(path)) {
			writer.WriteLine(grid.width);
			writer.WriteLine(grid.height);
			StringBuilder conditions = new StringBuilder("");
			if(LevelEditor.robotsWin) {
				conditions.Append("0 " + LevelEditor.robotLimit + " ");
			}
			if(LevelEditor.squareWins) {
				string[] coords = LevelEditor.winCoords.Split(new char[] {','});
				conditions.Append("1 " + Int32.Parse(coords[0]) + " " + Int32.Parse(coords[1]) + " ");
			}
			writer.WriteLine(conditions.ToString().Trim());
			writer.WriteLine("0 0 9 " + LevelEditor.audiofile);
			for(int i = 0; i < grid.width; i++) {
				for(int j = 0; j < grid.height; j++) {
					StringBuilder sb = new StringBuilder();
					sb.Append(i + " " + j + " "); // append the x and y coordinates of the grid Square in question
					Square sq = grid.grid[i, j];

					// if the Square contains a Wall, encode the relevant information
					GameObject obj = sq.objects.Find((GameObject g) => g.GetComponent<Wall>() != null);
					if(obj != null) {
						Wall wall = obj.GetComponent<Wall>();
						if(wall != null)
							sb.Append(0 + " ");
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
							sb.Append(6 + " " + robot.moveSpeed + " " + robot.fireRate +
								  " " + robot.damageDealt + " " + robot.health +
								  " " + robot.forwardRange + " " + robot.sideRange + " ");
							
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
							
							if(robot.colorPainted == Color.red)
								sb.Append(0 + " ");
							else if(robot.colorPainted == Color.green)
								sb.Append(1 + " ");
							else if(robot.colorPainted == Color.blue)
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
							
							if(robot.rotation == new RotationMatrix(RotationMatrix.Rotation.Identity))
								sb.Append(0 + " ");
							else if(robot.rotation == new RotationMatrix(RotationMatrix.Rotation.Left))
								sb.Append(1 + " ");
							else if(robot.rotation == new RotationMatrix(RotationMatrix.Rotation.Right))
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
						}
					}
					obj = sq.objects.Find((GameObject g) => g.GetComponent<DestructibleWall>() != null);
					if(obj != null) {
						DestructibleWall wall = obj.GetComponent<DestructibleWall>();
						if(wall != null) {
							sb.Append(7 + " " + wall.health + " ");
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
					obj = sq.objects.Find((GameObject g) => g.GetComponent<ExplosiveCrate>() != null);
					if(obj != null) {
						ExplosiveCrate exp = obj.GetComponent<ExplosiveCrate>();
						if(exp != null) {
							sb.Append(8 + " " + exp.health + " " + exp.range + " " + exp.damageDealt);
						}
					}
					obj = sq.objects.Find((GameObject g) => g.GetComponent<RobotSpawner>() != null);
					if(obj != null) {
						RobotSpawner spawner = obj.GetComponent<RobotSpawner>();
						if(spawner != null) {
							sb.Append(10 + " " + spawner.health + " " + spawner.spawnRate + " ");
							if(spawner.colorPainted == Color.red)
								sb.Append(0 + " ");
							else if(spawner.colorPainted == Color.green)
								sb.Append(1 + " ");
							else if(spawner.colorPainted == Color.blue)
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
							
							sb.Append(spawner.robotSpeed + " " + spawner.robotDamageDealt + " " +
								  spawner.robotHealth + " " + spawner.robotForwardRange + " " +
								  spawner.robotSideRange + " ");
							
							if(spawner.robotMovementDirection == new Vector2(0, 1))
								sb.Append(0 + " ");
							else if(spawner.robotMovementDirection == new Vector2(1, 0))
								sb.Append(1 + " ");
							else if(spawner.robotMovementDirection == new Vector2(0, -1))
								sb.Append(2 + " ");
							else if(spawner.robotMovementDirection == new Vector2(-1, 0))
								sb.Append(3 + " ");
							
							if(spawner.robotColorVisible == Color.red)
								sb.Append(0 + " ");
							else if(spawner.robotColorVisible == Color.green)
								sb.Append(1 + " ");
							else if(spawner.robotColorVisible == Color.blue)
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
							
							if(spawner.robotFireDirection == new Vector2(0, 1))
								sb.Append(0 + " ");
							else if(spawner.robotFireDirection == new Vector2(1, 0))
								sb.Append(1 + " ");
							else if(spawner.robotFireDirection == new Vector2(0, -1))
								sb.Append(2 + " ");
							else if(spawner.robotFireDirection == new Vector2(-1, 0))
								sb.Append(3 + " ");
							
							if(spawner.robotRotation == new RotationMatrix(RotationMatrix.Rotation.Identity))
								sb.Append(0 + " ");
							else if(spawner.robotRotation == new RotationMatrix(RotationMatrix.Rotation.Left))
								sb.Append(1 + " ");
							else if(spawner.robotRotation == new RotationMatrix(RotationMatrix.Rotation.Right))
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
							
							sb.Append(spawner.robotFireRate + " ");
							
							if(spawner.robotColorPainted == Color.red)
								sb.Append(0 + " ");
							else if(spawner.robotColorPainted == Color.green)
								sb.Append(1 + " ");
							else if(spawner.robotColorPainted == Color.blue)
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
							
							if(spawner.robotFireDirection == new Vector2(0, 1))
								sb.Append(0 + " ");
							else if(spawner.spawnDirection == new Vector2(1, 0))
								sb.Append(1 + " ");
							else if(spawner.spawnDirection == new Vector2(0, -1))
								sb.Append(2 + " ");
							else
								sb.Append(3 + " ");
						}
					}
					string line = sb.ToString().Trim();
					// Don't just write x and y coords if we don't need them
					if(line.Split(new char[] {' '}).Length > 2) {
						writer.WriteLine(line); // write all this crap to a line in the text file
					}
				}
			}
			// Lights
			foreach(GameObject obj in LevelEditor.lights) {
				Light l = obj.GetComponent<Light>();
				int x = (int)obj.transform.position.x;
				int y = (int)obj.transform.position.y;
				writer.WriteLine(x + " " + y + " " + 11 + " " + l.intensity + " " + l.range + " " + l.color.r + " " + l.color.g + " " + l.color.b);
			}
			writer.Close();
			Debug.Log("Saved to " + path +", bro.");
		}
	}
}
