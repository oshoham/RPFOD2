using UnityEngine;
using System.Collections.Generic;

public class Square {

	public Vector2 loc;	// (x,y) location coordinates within the Grid
	public List<GameObject> obj;	// list of GameObjects occupying this Square
	public Square[] bord;	// array of border Squares within the Grid
					// 0 = the Square to the bottom left, 7 = top right
	public Vector3 wloc;	// 3D location coordinates in Unity's world units
	public Plane plane;	// the actual plane or part of a plane that this Square represents
	public bool empt = true;


	// Constructor
	public Square(Grid gr, Vector2 loc, Vector3 wloc) {
		this.loc = loc;
		this.wloc = wloc;
		obj = new List<GameObject>();
		bord = new Square[8];
		int count = 0;
		for(int xof = -1; xof <= 1; xof++) {
			for(int yof = -1; yof <= 1; yof++) {
				int x = (int)loc.x;
				int y = (int)loc.y;
				if(!(xof == 0 && yof == 0)) {
					if(x+xof >= 0 && x+xof < gr.width && y+yof >= 0 && y+yof < gr.height)
						bord[count] = gr.grid[x+xof, y+yof];
					count++;
				}
			}
		}
	}

	public bool Move(Square sq, GameObject stuf)
	{// moves gameobject into given square, returns if this square is empty
		if(obj.Contains(stuf))
		{
			obj.Remove(stuf);
			sq.obj.Add(stuf);
			sq.empt = false;
			empt = ((obj.Count == 0) ? true : false);
			return true;
		}
		return false;		
	}

	void Start() {

	}

	void Update() {

	}
}
