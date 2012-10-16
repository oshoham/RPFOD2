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
				//Vector2 coordinates = new Vector2(i, j);
				//grid[i][j] = new Square(grid, coordinates, whatgoeshere?);
			}
		}
	}

	void Start() {

	}

	void Update() {

	}

	// Check to see if the Sqaure at the given coordinates is occupied by anything
	bool Check(Vector2 coord) {
		if(grid[(int)coord.x, (int)coord.y].objects.Count > 0)
			return true;
		return false;
	}
	
	// Check to see if the Square at the given coordinates is occupied by a Wall
	/*bool CheckWall(Vector2 coord) {
		Square sq = grid[(int)coord.x, (int)coord.y];
		if(sq.objects.Count > 0) {
			foreach(GameObject g in sq.objects) {
				if(g.GetComponent<Wall>() != null)
					return true;
			}
		}
		return false;
	}*/

	// Get a GameObject of a particular type at a particular (x,y) if there is one
	GameObject Get(Vector2 coord, string type) {
		Square sq = grid[(int)coord.x, (int)coord.y];
		if(sq.objects.Count > 0) {
			foreach(GameObject g in sq.objects) {
				if(g.GetComponent(type) != null)
					return g;
			}
		}
		return null;
	}

	// Move a given GameObject from one Square to another (does not handle animation)
	void Move(Vector2 start, Vector2 end, GameObject mover) {
		Square sq1 = grid[(int)start.x, (int)start.y];
		Square sq2 = grid[(int)end.x, (int)end.y];
		if(sq1.objects.Count == 0)
			return;
		foreach(GameObject g in sq1.objects) {
			if(g == mover) {
				sq2.objects.Add(g);
				sq1.objects.Remove(g);
				break;
			}
		}
	}
}
