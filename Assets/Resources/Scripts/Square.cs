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
	// private Color _colorPainted;
	// public Color colorPainted{ get{return _colorPainted;}
	// 			   set {
	// 				   plane.renderer.material.color = value;
	// 				   _colorPainted = value;
	// 			   }
	// 			 }


	// Constructor
	public Square(Grid gr, Vector2 loc, Vector3 wloc) {
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
	
	// public void ChangeColor(Color col, int n) {
	// 	colors[col] += n;
	// 	SetColor();
	// }
	
	/*
	 * holy fuck this is hacky
	 *
	 * also doesn't work for levels other than the current one
	 */
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
}
