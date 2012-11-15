using UnityEngine;
using System;
using System.Collections.Generic;

// worst class name ever? maybe?
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
				grid.Add(Wall.MakeWall(grid, x, y, LevelEditor.wallColor), x, y);
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
								 Int32.Parse(LevelEditor.robotDamageDealt),
								 Int32.Parse(LevelEditor.robotHealth),
								 Int32.Parse(LevelEditor.robotForwardRange),
								 Int32.Parse(LevelEditor.robotSideRange),
								 LevelEditor.robotMovementDirection,
								 LevelEditor.robotColorVisible, LevelEditor.robotFireDirection,
								 LevelEditor.robotRotation),
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
									   Int32.Parse(LevelEditor.explosiveCrateRange)),
					 x, y);
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
