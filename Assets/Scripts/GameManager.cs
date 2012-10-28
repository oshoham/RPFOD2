using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 40;
	public static readonly int HEIGHT = 40;

	public static Grid floor;

	void Start() {
		floor = new Grid(WIDTH, HEIGHT);
		// this bit is kinda silly, don't set the color later
		Player.MakePlayer(0, 0, 15, new Vector3(0.0f, 0.0f, 0.0f)).GetComponent<Player>().SetColorPainted(Color.green);
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
