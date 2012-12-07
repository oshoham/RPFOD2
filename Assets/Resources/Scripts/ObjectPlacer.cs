using UnityEngine;
using System;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour {

	public int x, y;
	public Grid grid;
	
	/*
	 * If we get a right click, clear this square.
	 */
	void OnMouseOver() {
		if(Input.GetMouseButtonDown(1)) {
			grid.grid[x, y].ClearObjects();
		}
	}
	
	void OnMouseDown() {
		switch(LevelEditor.objectToBeCreated) {
			case ObjectType.Wall:
				grid.Add(Wall.MakeWall(grid, x, y), x, y);
				break;
			case ObjectType.SpikeWall:
				grid.Add(SpikeWall.MakeSpikeWall(grid, x, y,
								 LevelEditor.spikeWallDirections,
								 LevelEditor.spikeWallColor),
					 x, y);
				break;
			case ObjectType.SpikeFloor:
				grid.Add(SpikeFloor.MakeSpikeFloor(grid, x, y), x, y);
				break;
			case ObjectType.Paint:
				try{
					grid.Add(Paint.MakePaint(grid, x, y, LevelEditor.paintColor, Single.Parse(LevelEditor.paintRespawnTime)), x, y);
				}
				catch(FormatException) {
					print("Number format exception for Paint respawn time!");
				}
				break;
			case ObjectType.Conveyor:
				try {
					grid.Add(Conveyor.MakeConveyor(grid, new Vector2(x, y), LevelEditor.conveyorDirection,
								       Single.Parse(LevelEditor.conveyorLength),
								       Single.Parse(LevelEditor.conveyorSpeed), LevelEditor.conveyorSwitchable,
								       Single.Parse(LevelEditor.conveyorSwitchRate)),
						 x, y);
				}
				catch(FormatException) {
					print("Number format exception for Conveyor... somewhere!");
				}
				break;
			case ObjectType.Player:
				try {
					print("Placing player: " + x + " " + y);
					grid.Add(Player.MakePlayer(grid, x, y, Int32.Parse(LevelEditor.playerHealth)), x, y);
					print("placed player");
					foreach(GameObject obj in grid.GetObjectsOfTypes(new Vector2(x, y), new List<string>() {"Player"})) {
						print(obj.GetComponent<Player>());
					}
				}
				catch(FormatException) {
					print("Number format exception for Player health!");
				}
				break;	
			case ObjectType.Robot:
				try {
					grid.Add(Robot.MakeRobot(grid, x, y, Single.Parse(LevelEditor.robotSpeed),
								 Single.Parse(LevelEditor.robotFireRate),
								 Int32.Parse(LevelEditor.robotDamageDealt),
								 Int32.Parse(LevelEditor.robotHealth),
								 Int32.Parse(LevelEditor.robotForwardRange),
								 Int32.Parse(LevelEditor.robotSideRange),
								 LevelEditor.robotMovementDirection,
								 LevelEditor.robotColorVisible,
								 LevelEditor.robotColorPainted,
								 LevelEditor.robotFireDirection,
								 new RotationMatrix(LevelEditor.robotRotation)),
						 x, y);
				}
				catch(FormatException) {
					print("Number format exception for Roobitt... somewhere!");
				}
				break;
			case ObjectType.DestructibleWall:
				try {
					grid.Add(DestructibleWall.MakeDestructibleWall(grid, x, y,
										       Int32.Parse(LevelEditor.destructibleWallHealth),
										       LevelEditor.destructibleWallColor),
						 x, y);
				}
				catch(FormatException) {
					print("Number format exception for DestructibleWall health!");
				}
				break;
			case ObjectType.ExplosiveCrate:
				grid.Add(ExplosiveCrate.MakeExplosiveCrate(grid, x, y,
									   Int32.Parse(LevelEditor.explosiveCrateHealth),
									   Int32.Parse(LevelEditor.explosiveCrateRange),
									   Int32.Parse(LevelEditor.explosiveCrateDamage)),
					 x, y);
				break;
			case ObjectType.RobotSpawner:
				grid.Add(RobotSpawner.MakeRobotSpawner(grid, x, y,
								       Int32.Parse(LevelEditor.robotSpawnerHealth),
								       Single.Parse(LevelEditor.robotSpawnerSpawnRate),
								       LevelEditor.robotSpawnerColorPainted,
								       Single.Parse(LevelEditor.robotSpawnerRobotSpeed),
								       Int32.Parse(LevelEditor.robotSpawnerRobotDamageDealt),
								       Int32.Parse(LevelEditor.robotSpawnerRobotHealth),
								       Int32.Parse(LevelEditor.robotSpawnerRobotForwardRange),
								       Int32.Parse(LevelEditor.robotSpawnerRobotSideRange),
								       LevelEditor.robotSpawnerRobotMovementDirection,
								       LevelEditor.robotSpawnerRobotColorVisible,
								       LevelEditor.robotSpawnerRobotFireDirection,
								       new RotationMatrix(LevelEditor.robotSpawnerRobotRotation),
								       Single.Parse(LevelEditor.robotSpawnerRobotFireRate),
								       LevelEditor.robotSpawnerRobotColorPainted,
								       LevelEditor.robotSpawnerSpawnDirection),
					 x, y);
					 break;
			case ObjectType.Light:
				GameObject obj = new GameObject("A light!");
				obj.transform.position = new Vector3(x, y, -1);
				Light light = obj.AddComponent<Light>();
				light.range = Single.Parse(LevelEditor.lightRange);
				light.intensity = Single.Parse(LevelEditor.lightIntensity);
				string[] color = LevelEditor.lightColor.Split(new char[] {','});
				light.color = new Color(Int32.Parse(color[0]), Int32.Parse(color[1]), Int32.Parse(color[2]), 1);
				LevelEditor.lights.Add(obj);
				break;
		}
	}

	public static GameObject MakeObjectPlacer(int x, int y, Grid grid) {
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.transform.Rotate(-90, 0, 0);
		plane.transform.localScale = new Vector3(0.1f, 1, 0.1f);
		plane.transform.position = new Vector3(x, y, -1.0f); // get it in front of Squares
		plane.renderer.enabled = false; // make it invisible
		plane.collider.enabled = true;
		plane.name = "Object Placer";
		ObjectPlacer script = plane.AddComponent<ObjectPlacer>();
		script.x = x;
		script.y = y;
		script.grid = grid;
		script.collider.enabled = true;
		return plane;
	}
}
