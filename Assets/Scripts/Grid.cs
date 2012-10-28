using UnityEngine;
using System.Collections.Generic;
using System;

public class Grid {

	public Square[,] grid; // 2D array of Squares that underlies the Grid class
	public int width;
	public int height;

	// Constructor
	public Grid(int width, int height) {
		this.width = width;
		this.height = height;
		grid = new Square[(int)width, (int)height];
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.renderer.material.color = Color.black;
		plane.transform.localScale = new Vector3(width/10.0f, 1.0f, height/10.0f);
		plane.transform.Rotate(-90.0f, 0.0f, 0.0f);
		plane.transform.Translate(width/2.0f - 0.5f, 0.0f, height/2.0f - 0.5f);
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				Vector2 loc = new Vector2(i, j);
				grid[i, j] = new Square(this, loc, new Vector3(i, j, 0.0f));
			}
		}
	}
	
	// Check to see if the Sqaure at the given loc is occupied by anything other than Paint.
	public bool Check(Vector2 loc) {
		if(!CheckCoords(loc)) {
			return true;
		}
		// Checks if there are any non-paint objects in the square.
		if(grid[(int)loc.x, (int)loc.y].objects.Find(
							     delegate (GameObject obj) {
								     return obj.GetComponent<Paint>() == null;
							     })
		   != null)
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
		if(sq.objects.Count > 0) {
			foreach(GameObject g in sq.objects) {
				if(g.GetComponent<Wall>() != null)
					return true;
			}
		}
		return false;
	}

	// Get a GameObject of a particular type at a particular (x,y) if there is one
	public GameObject Get(Vector2 loc, string type) {
		Square sq = grid[(int)loc.x, (int)loc.y];
		if(sq.objects.Count > 0) {
			foreach(GameObject g in sq.objects) {
				if(g.GetComponent(type) != null)
					return g;
			}
		}
		return null;
	}

	/*
	 * Returns a List of all GameObjects in a Square that are of the types passed
	 * in as classList.
	 */
	public List<GameObject> GetObjectsOfTypes(Vector2 location, List<Type> classList) {
		List<GameObject> objects = grid[(int)location.x, (int)location.y].objects;
		return objects.FindAll(
				       delegate (GameObject obj) {
					       foreach(Type towels in classList) {
						       // if(obj.GetComponent<typeof(towels)>() != null) {
						       // 	       return true;
						       // }
					       }
					       return false;
				       });
	}

	// Move a given GameObject from one Square to another (does not handle animation)
	public bool Move(Vector2 start, Vector2 end, GameObject mover) {
		Square sq1 = grid[(int)start.x, (int)start.y];
		Square sq2 = grid[(int)end.x, (int)end.y];
		return sq1.Move(sq2, mover);
	}

	/*
	 * Checks if a given Vector2 is outside of the range of the board. Returns
	 * true if the coordinates are OK.
	 */
	public bool CheckCoords(Vector2 coords) {
		return (coords.x >= 0 && coords.x < grid.GetLength(0) &&
			coords.y >= 0 && coords.y < grid.GetLength(1));
	}
	
	/*
	 * Adds the given GameObject to the grid at (x, y).
	 */
	public void Add(GameObject obj, int x, int y) {
		grid[x, y].objects.Add(obj);
	}

	/*
	 * Removes the given GameObject from the grid at (x, y).
	 */
	public void Remove(GameObject obj, int x, int y) {
		grid[x, y].objects.Remove(obj);
	}
}
