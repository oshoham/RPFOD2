using UnityEngine;
using System.Collections.Generic;

public class Square {

	public Vector2 loc;	// (x,y) location coordinates within the Grid
	public List<GameObject> objects;	// list of GameObjects occupying this Square
	public Square[] board;	// array of boarder Squares within the Grid
					// 0 = the Square to the bottom left, 7 = top right
	public Vector3 wloc;	// 3D location coordinates in Unity's world units
	public GameObject plane;	// the actual plane or part of a plane that this Square represents
	public bool empt = true;


	// Constructor
	public Square(Grid gr, Vector2 loc, Vector3 wloc) {
		this.loc = loc;
		this.wloc = wloc;
		objects = new List<GameObject>();
		board = new Square[8];
		int count = 0;
		for(int xof = -1; xof <= 1; xof++) {
			for(int yof = -1; yof <= 1; yof++) {
				int x = (int)loc.x;
				int y = (int)loc.y;
				if(!(xof == 0 && yof == 0)) {
					if(x+xof >= 0 && x+xof < gr.width && y+yof >= 0 && y+yof < gr.height)
						board[count] = gr.grid[x+xof, y+yof];
					count++;
				}
			}
		}
	}
	
	/*
	 * Returns true if the GameObject obj is moved, false otherwise.
	 */
	public bool Move(Square sq, GameObject obj) {
		if(objects.Contains(obj)) {
			objects.Remove(obj);
			sq.objects.Add(obj);
			sq.empt = false;
			sq.Pickup(obj);
			empt = ((objects.Count == 0) ? true : false);
			return true;
		}
		return false;
	}

	/*
	 * Checks if this square contains any items of a pick-up-able type,
	 * and if so, gives them to the GameObject obj.
	 */
	public void Pickup(GameObject obj) {
		// Paint
		Player player = obj.GetComponent<Player>();
		if(player) {
			GameObject paint = objects.Find(
							delegate (GameObject o) {
								return o.GetComponent<Paint>() != null;
							});
			if(paint) {
				player.PickupColor(paint.GetComponent<Paint>().colorPainted);
				GameManager.floor.Remove(paint, (int)loc.x, (int)loc.y);
				Object.Destroy(paint);
			}
		}
	}
}
