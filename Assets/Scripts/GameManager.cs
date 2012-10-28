using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 40;
	public static readonly int HEIGHT = 40;

	public static Grid floor;

	void Start() {
		floor = new Grid(WIDTH, HEIGHT);
		Player.MakePlayer(0, 0, 15);
		Paint.MakePaint(5, 5, Color.red);
		Paint.MakePaint(7, 5, Color.green);
		Paint.MakePaint(14, 2, Color.blue);
		GameObject light = new GameObject("Light");
		Light l = light.AddComponent<Light>();
		light.transform.position = Camera.main.transform.position;
		l.type = LightType.Directional;
		l.intensity = 0.2f;
	}

	void Update() {

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
