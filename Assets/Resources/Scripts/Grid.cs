using UnityEngine;
using System;
using System.Collections.Generic;

public class Grid {

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
				Square sq = new Square(this, loc, new Vector3(i, j, 0.0f));
				grid[i, j] = sq;
			}
		}
	}
	
	/*
	 * Check to see if the Square at the given loc is occupied by anything other than Paint.
	 * Returns true if the square shouldn't be moved into.
	 */
	public bool Check(Vector2 loc) {
		if(!CheckCoords(loc)) {
			return true;
		}
		// Checks if there are any objects in the square that should be avoided.
		if(grid[(int)loc.x, (int)loc.y].objects.Find((GameObject obj) => {
					return obj.GetComponent<Paint>() == null &&
					obj.GetComponent<SpikeFloor>() == null &&
					obj.GetComponent<Conveyor>() == null;
				})
			!= null)
			return true;
		return false;
	}
	
	/*
	 * Checks all grid coordinates between origin and coord and returns a list of
	 * the objects. It'll be empty if there's nothing. TODO: make this less hacky.
	 *
	 * ... actually it's not that terrible
	 */
	public List<GameObject> CheckLine(Vector2 origin, Vector2 coord) {
		Vector2 diff = coord - origin;
		List<GameObject> objects = new List<GameObject>();
		FixVector(ref coord);
		if(diff.x == 0) { // we're checking in the y direction
			int sign = diff.y < 0 ? -1 : 1; // which way are we going?
			origin.y += sign;
			while((sign == 1 ? origin.y <= coord.y : origin.y >= coord.y) &&
			      GetObjectsOfTypes(origin, new List<String>() {"Wall"}).Count == 0) { // we want the last position as well so it's a do-while loop
				Square sq = grid[(int)origin.x, (int)origin.y];
				objects.AddRange(sq.objects);
				if(GetObjectsOfTypes(origin, new List<String>() {"DestructibleWall", "ExplosiveCrate"}).Count > 0)
					break;
				origin.y += sign;
			}
		}
		else { // checking y, otherwise the same
			int sign = diff.x < 0 ? -1 : 1;
			origin.x += sign;
			while((sign == 1 ? origin.x <= coord.x : origin.x >= coord.x) &&
			      GetObjectsOfTypes(origin, new List<String>() {"Wall"}).Count == 0) {
				objects.AddRange(grid[(int)origin.x, (int)origin.y].objects);
				if(GetObjectsOfTypes(origin, new List<String>() {"DestructibleWall", "ExplosiveCrate"}).Count > 0)
					break;
				origin.x += sign;
			}
		}
		return objects;
	}
	
	/*
	 * Similar to CheckLine, but returns Squares instead of GameObjects.
	 */
	public List<Square> SCheckLine(Vector2 origin, Vector2 coord) {
		Vector2 diff = coord - origin;
		List<Square> objects = new List<Square>();
		FixVector(ref coord);
		if(diff.x == 0) { // we're checking in the y direction
			int sign = diff.y < 0 ? -1 : 1; // which way are we going?
			origin.y += sign;
			while((sign == 1 ? origin.y <= coord.y : origin.y >= coord.y) &&
			      GetObjectsOfTypes(origin, new List<String>() {"Wall", "DestructibleWall", "ExplosiveCrate"}).Count == 0) {
				objects.Add(grid[(int)origin.x, (int)origin.y]);
				origin.y += sign;
			}
		}
		else { // checking y, otherwise the same
			int sign = diff.x < 0 ? -1 : 1;
			origin.x += sign;
			while((sign == 1 ? origin.x <= coord.x : origin.x >= coord.x) &&
			      GetObjectsOfTypes(origin, new List<String>() {"Wall", "DestructibleWall", "ExplosiveCrate"}).Count == 0) {
				objects.Add(grid[(int)origin.x, (int)origin.y]);
				origin.x += sign;
			}
		}
		return objects;
	}

	/*
	 * Checks radius for squares that aren't walls
	 */
	public List<Square> SCheckRad(int rad, Vector2 origin) {
		Vector2 scoord = new Vector2(origin.x-rad, origin.y-rad);
		Vector2 ecoord = new Vector2(origin.x+rad, origin.y+rad);
		FixVector(ref scoord);
		FixVector(ref ecoord);
		int ex = (int)ecoord.x;
		int ey = (int)ecoord.y;
		int sx = (int)scoord.x;
		int sy = (int)scoord.y;
		Square[,] see = new Square[ex-sx+1, ey-sy+1];
		Square osquare = grid[(int)origin.x, (int)origin.y];

		for(int i = 0; i < see.GetLength(0); i++) {
			for(int j = 0; j < see.GetLength(1); j++) {
				if(i == rad && j == rad) {
					see[i,j] = null;
					continue;
				}
				if(sx+i <= ex) {
					if(sy+j <= ey)
						see[i,j] = grid[(int)scoord.x + i,(int)scoord.y + j];
					else
						see[i,j] = grid[(int)scoord.x + i,(int)scoord.y];
				}
				else if(sy + j <= ey)
					see[i,j] = grid[(int)scoord.x,(int)scoord.y + j];
				
			}
		}
	
		List<Square> fsee = new List<Square>();

		for(int i = 0; i < see.GetLength(0); i++) {
			for(int j = 0; j < see.GetLength(1); j++) {
				if(see[i,j] != null){
					RaycastHit[] hits = Physics.RaycastAll(osquare.wloc, 
										(see[i,j].wloc - osquare.wloc).normalized, 
										Vector3.Distance(osquare.wloc, see[i,j].wloc));
				bool plus;	
				if(Array.Find(hits,(RaycastHit hit) => { 
						return hit.transform.gameObject.GetComponent<Wall>() != null; })
					.Equals(default(RaycastHit)))
					plus = true;
				else
					plus = false;


					/*foreach(RaycastHit block in hits) {
						GameObject obj = block.transform.gameObject;
						if(obj.GetComponent<Wall>() == null &&
							plus = true;
						}
						else {
							plus = false;
							break;
						}

					}*/
					

					if(plus)
						fsee.Add(see[i, j]);
				}
			}
		}
		return fsee;
	}
	

	/*
	 * Makes sure a Vector2's coordinates are in the right range and
	 * puts them in the right range if they aren't.
	 */
	public void FixVector(ref Vector2 v) {
		if(v.x < 0) {
			v.x = 0;
		}
		if(v.y < 0) {
			v.y = 0;
		}
		if(v.x >= grid.GetLength(0)) {
			v.x = grid.GetLength(0) - 1;
		}
		if(v.y >= grid.GetLength(1)) {
			v.y = grid.GetLength(1) - 1;
		}
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
	public List<GameObject> GetObjectsOfTypes(Vector2 location, List<string> classList) {
		List<GameObject> objects = grid[(int)location.x, (int)location.y].objects;
		return objects.FindAll(
				       delegate (GameObject obj) {
					       foreach(String t in classList) {
						       if(obj.GetComponent(t) != null) {
						       	       return true;
						       }
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
	
	/*
	 * Clears the whole grid by removing all game objects from each square and
	 * destroying all the square's planes.
	 */
	public void Clear() {
		foreach(Square sq in grid) {
			sq.Clear();
		}
	}
	
	/*
	 * Similar to Clear(), but leaves planes intact.
	 */
	public void ClearObjects() {
		foreach(Square sq in grid) {
			sq.ClearObjects();
		}
	}
	
	/*
	 * Makes a copy of this grid. It does NOT clear out the old grid, because
	 * we don't want to destroy the game objects contained within that grid.
	 */
	public Grid Copy(int width, int height) {
		Grid copy = new Grid(width, height);
		for(int i = 0; i < width; i++) {
			for(int j = 0; j < height; j++) {
				if(i < grid.GetLength(0) && j < grid.GetLength(1)) {
					copy.grid[i, j].objects = grid[i, j].objects;
					UnityEngine.Object.Destroy(grid[i, j].plane);
				}
			}
		}
		for(int i = 0; i < grid.GetLength(0); i++) {
			for(int j = 0; j < grid.GetLength(1); j++) {
				if(i >= width || j >= height) {
					grid[i, j].Clear();
				}
			}
		}
		return copy;
	}
}
