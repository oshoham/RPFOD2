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
				grid.Add(Wall.MakeWall(grid, x, y, LevelEditor.wallHealth, LevelEditor.wallDestructible, LevelEditor.wallColor), x, y);
				break;
			case ObjectType.SpikeWall:
				grid.Add(SpikeWall.MakeSpikeWall(grid, x, y, LevelEditor.spikeWallHealth, LevelEditor.spikeWallDestructible,
								 LevelEditor.spikeWallDirections, LevelEditor.spikeWallColor), x, y);
				break;
			case ObjectType.SpikeFloor:
				grid.Add(SpikeFloor.MakeSpikeFloor(grid, x, y), x, y);
				break;
			case ObjectType.Paint:
				grid.Add(Paint.MakePaint(grid, x, y, LevelEditor.paintColor, LevelEditor.paintRespawnTime), x, y);
				break;
			case ObjectType.Conveyor:
				Conveyor.MakeConveyor(grid, new Vector2(x, y), LevelEditor.conveyorDirection, LevelEditor.conveyorLength,
						      LevelEditor.conveyorSpeed, LevelEditor.conveyorSwitchable,
						      LevelEditor.conveyorSwitchRate);
				break;
			case ObjectType.Player:
				print("Placing player: " + x + " " + y);
				grid.Add(Player.MakePlayer(grid, x, y, LevelEditor.playerHealth), x, y);
				print("placed player");
				foreach(GameObject obj in grid.GetObjectsOfTypes(new Vector2(x, y), new List<string>() {"Player"})) {
					print(obj.GetComponent<Player>());
				}
				break;	
			case ObjectType.Robot:
				grid.Add(Robot.MakeRobot(grid, x, y, LevelEditor.robotSpeed, LevelEditor.robotDamageDealt, LevelEditor.robotHealth,
							 LevelEditor.robotForwardRange, LevelEditor.robotSideRange, LevelEditor.robotMovementDirection,
							 LevelEditor.robotColorVisible, LevelEditor.robotFireDirection, LevelEditor.robotTurnsLeft),
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