using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Square {

	public Vector2 loc;	// (x,y) location coordinates within the Grid
	public List<GameObject> objects;	// list of GameObjects occupying this Square
	public Square[] board;	// array of boarder Squares within the Grid
					// 0 = the Square to the bottom left, 7 = top right
	public Vector3 wloc;	// 3D location coordinates in Unity's world units
	public GameObject plane;	// the actual plane or part of a plane that this Square represents
	public bool empt = true;
	public Dictionary<Color, int> colors;
	
	public Square(Grid gr, Vector2 loc, Vector3 wloc) {
		this.plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.name = "Grid plane";
		plane.transform.position = wloc;
		plane.transform.localScale = new Vector3(0.1f, 1.0f, 0.1f);
		plane.transform.Rotate(-90.0f, 0.0f, 0.0f);
		plane.renderer.material.mainTexture = Resources.Load("Textures/Tile2") as Texture;
		plane.renderer.material.color = Color.white;
		colors = new Dictionary<Color, int>();
		colors[Color.red] = 0;
		colors[Color.green] = 0;
		colors[Color.blue] = 0;
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
	
	// Add green!
	public void SetColor() {
		if(colors[Color.red] > 0 && colors[Color.blue] > 0) {
			plane.renderer.material.color = new Color(1, 0, 1, 1);
		}
		else if(colors[Color.red] > 0) {
			plane.renderer.material.color = Color.red;
		}
		else if(colors[Color.blue] > 0) {
			plane.renderer.material.color = Color.blue;
		}
		else
			plane.renderer.material.color = Color.white;
	}
	
	/*
	 * Returns true if the GameObject obj is moved, false otherwise.
	 */
	public bool Move(Square sq, GameObject obj) {
		if(objects.Contains(obj)) {
			objects.Remove(obj);
			sq.objects.Add(obj);
			sq.empt = false;
			empt = ((objects.Count == 0) ? true : false);
			return true;
		}
		return false;
	}
	
	/*
	 * Destroys all game objects on this square.
	 */
	public void Clear() {
		for(int i = 0; i < objects.Count; i++) {
			Object.Destroy(objects[i]);
		}
		Object.Destroy(plane);
	}
}
