using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

	Vector2 coordinates;	// (x,y) coordinates within the Grid
	List<GameObject> objects;	// list of GameObjects occupying this Square
	Square[] neighbors;	// array of adjacent Squares within the Grid, with 0 being the Square to the bottom left and 7 being the Square to the top right (arbitrarily decided upon, subject to change)
	Vector3 worldCoordinates;	// 3D coordinates in Unity's world units
	Plane plane;	// the actual plane or part of a plane that this Square represents

	// Constructor
	public Square(Grid gr, Vector2 coordinates, Vector3 worldCoordinates) {
		this.coordinates = coordinates;
		this.worldCoordinates = worldCoordinates;
		objects = new List<GameObject>();
		neighbors = new Square[8];
		int count = 0;
		for(int xoffset = -1; xoffset <= 1; xoffset++) {
			for(int yoffset = -1; yoffset <= 1; yoffset++) {
				if(!(xoffset == 0 && yoffset == 0)) {
					if(coordinates.x+xoffset >= 0 && coordinates.x+xoffset < gr.width && coordinates.y+yoffset >= 0 && coordinates.y+yoffset < gr.height)
						neighbors[count] = gr.grid[coordinates.x+xoffset, coordinates.y+yoffset];
					count++;
				}
			}
		}
		//plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		//plane.transform.position = worldCoordinates;
	}

	void Start() {

	}

	void Update() {

	}
}
