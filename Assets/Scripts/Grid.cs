using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public Square[,] grid; // 2D array of Squares that underlies the Grid class
	public int width;
	public int height;

	// Constructor
	public Grid(int width, int height) {
		this.width = width;
		this.height = height;
		grid = new Square[(int)width, (int)height];

		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				Vector2 loc = new Vector2(i, j);
				grid[i, j] = new Square(this, loc, new Vector3(i, j, 0.0f));
			}
		}
	}

	void Start() {

	}

	void Update() {

	}

	// Check to see if the Sqaure at the given loc is occupied by anything
	public bool Check(Vector2 loc) {
		if(grid[(int)loc.x, (int)loc.y].obj.Count > 0)
			return true;
		return false;
	}
	
	/*
	 * Checks all grid coordinates between origin and coord and returns a list of
	 * the objects TODO
	 */
	public List<GameObject> CheckLine(Vector2 origin, Vector2 coord) {
		return null;
	}
	
	// Check to see if the Square at the given loc is occupied by a Wall
	public bool CheckWall(Vector2 loc) {
		Square sq = grid[(int)loc.x, (int)loc.y];
		if(sq.obj.Count > 0) {
			foreach(GameObject g in sq.obj) {
				if(g.GetComponent<Wall>() != null)
					return true;
			}
		}
		return false;
	}

	// Get a GameObject of a particular type at a particular (x,y) if there is one
	public GameObject Get(Vector2 loc, string type) {
		Square sq = grid[(int)loc.x, (int)loc.y];
		if(sq.obj.Count > 0) {
			foreach(GameObject g in sq.obj) {
				if(g.GetComponent(type) != null)
					return g;
			}
		}
		return null;
	}

	// Move a given GameObject from one Square to another (does not handle animation)
	public bool Move(Vector2 start, Vector2 end, GameObject mover) {
		Square sq1 = grid[(int)start.x, (int)start.y];
		Square sq2 = grid[(int)end.x, (int)end.y];
		return sq1.Move(sq2, mover);
	}
}
