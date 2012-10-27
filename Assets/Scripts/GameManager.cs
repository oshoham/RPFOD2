using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 400;
	public static readonly int HEIGHT = 400;

	public static Grid floor;

	void Start() {
		floor = new Grid(WIDTH, HEIGHT);
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
