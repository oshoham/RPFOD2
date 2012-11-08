using UnityEngine;
using System;

// worst class name ever? maybe?
public class ObjectPlacer : MonoBehaviour {

	public int x, y;
	public Grid grid;

	void OnMouseDown() {
		switch(LevelEditor.objectToBeCreated) {
			case ObjectType.Wall:
				grid.Add(Wall.MakeWall(x, y, LevelEditor.wallHealth, LevelEditor.wallDestructible, LevelEditor.wallColor), x, y);
				break;
			case ObjectType.SpikeWall:
				grid.Add(SpikeWall.MakeSpikeWall(x, y, LevelEditor.spikeWallHealth, LevelEditor.spikeWallDestructible,
								 LevelEditor.spikeWallDirections, LevelEditor.spikeWallColor), x, y);
				break;
			case ObjectType.SpikeFloor:
				grid.Add(SpikeFloor.MakeSpikeFloor(x, y), x, y);
				break;
			case ObjectType.Paint:
				grid.Add(Paint.MakePaint(x, y, LevelEditor.paintColor, LevelEditor.paintRespawnTime), x, y);
				break;
			case ObjectType.Conveyor:
				Conveyor.MakeConveyor(new Vector2(x, y), LevelEditor.conveyorDirection, LevelEditor.conveyorLength,
						      LevelEditor.conveyorSpeed, LevelEditor.conveyorSwitchable,
						      LevelEditor.conveyorSwitchRate);
				break;
			case ObjectType.Player:
				grid.Add(Player.MakePlayer(x, y, LevelEditor.playerHealth), x, y);
				break;	
			case ObjectType.Robot:
				grid.Add(Robot.MakeRobot(x, y, LevelEditor.robotSpeed, LevelEditor.robotDamageDealt, LevelEditor.robotHealth,
							 LevelEditor.robotForwardRange, LevelEditor.robotSideRange, LevelEditor.robotMovementDirection,
							 LevelEditor.robotColorVisible, LevelEditor.robotFireDirection, LevelEditor.robotTurnsLeft),
					 x, y);
				break;
		}
	}

	public static GameObject MakeObjectPlacer(int x, int y, Grid grid) {
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.transform.position = new Vector3(x, y, -1.0f); // get it in front of Squares
		plane.renderer.enabled = false; // make it invisible
		ObjectPlacer script = plane.AddComponent<ObjectPlacer>();
		script.x = x;
		script.y = y;
		script.grid = grid;
		return plane;
	}
}