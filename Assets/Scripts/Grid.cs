using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	Square[][] grid;

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
}
