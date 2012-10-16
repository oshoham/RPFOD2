using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	Square[][] grid;
	int width;
	int height;

	// Constructor
	public Grid(int width, int height) {
		this.width = width;
		this.height = height;
		grid = new Square[width][height];
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				Vector2 coordinates = new Vector2(i, j);
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
		if(grid[coord.x][coord.y].objects.Count > 0)
			return true;
		return false;
	}
	
	// Check to see if the Square at the given coordinates is occupied by a Wall
	bool CheckWall(Vector2 coord) {
		Square sq = grid[coord.x][coord.y];
		if(sq.objects.Count > 0) {
			foreach(GameObject g in sq.objects) {
				if(g.GetComponent<Wall>() != null)
					return true;
			}
		}
		return false;
	}

	// Get a GameObject of a particular type at a particular (x,y) if there is one
	GameObject Get(Vector2 coord, String type) {
		Square sq = grid[coord.x][coord.y];
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
		Square sq1 = grid[start.x][start.y];
		Square sq2 = grid[end.x][end.y];
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
