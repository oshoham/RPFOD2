using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 40;
	public static readonly int HEIGHT = 40;

	public static Grid floor;
	public static Player player;

	void Start() {
		floor = new Grid(WIDTH, HEIGHT);
		player = Player.MakePlayer(0, 0, 15).GetComponent<Player>();
		Paint.MakePaint(5, 5, Color.red, 5.0f);
		Paint.MakePaint(7, 5, Color.green, 1.0f);
		Paint.MakePaint(7, 1, Color.blue, 2.0f);
		Robot.MakeRobot(x: 5, y: 1, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 10, sideRange: 3, movementDirection: new Vector2(1, 0),
				colorVisible: Color.green, turnsLeft: true);
		//		SpikeWall.MakeSpikeWall(x: 9, y: 1, health: 5, destructible: true, directions: new List<Vector2> {new Vector2(1, 0)}, color: Color.green);
		//SpikeFloor.MakeSpikeFloor(11, 1);
		//Wall.MakeWall(x: 15, y: 1, health: 5, destructible: true, color: Color.green);
		PlayerGui.MakePlayerGui(Color.red, new Vector3(20.0f, Camera.main.pixelHeight - 50.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(Color.green, new Vector3(50.0f, Camera.main.pixelHeight - 50.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(Color.blue, new Vector3(80.0f, Camera.main.pixelHeight - 50.0f, Camera.main.nearClipPlane + 5.0f), true);
		GameObject light = new GameObject("Light");
		Light l = light.AddComponent<Light>();
		light.transform.position = Camera.main.transform.position;
		l.type = LightType.Directional;
		l.intensity = 0.2f;
		Conveyor.MakeConveyor(new Vector2(0, 0), new Vector2(1, 0), 6, 0.1f);
		Conveyor.MakeConveyor(new Vector2(7, 0), new Vector2(0, 1), 5, 0.1f);
	}

	void Update() {

	}
	
	void OnGUI() {
		GUI.Label(new Rect(10, 10, 100, 50), "Health: " + player.health);
	}
	
	/*
	 * Move a GameObject mover from start to end. Returns true if the object
	 * actually ended up moving, false otherwise.
	 */
	public static bool Move(Vector2 start, Vector2 end, GameObject mover) {
		if(!floor.Check(end)) {
			floor.Move(start, end, mover);
			return true;
		}
		return false;
	}
}
