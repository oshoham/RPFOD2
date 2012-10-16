using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

	Vector2 coordinates;	// (x,y) coordinates within the Grid
	List<GameObject> objects;	// list of GameObjects occupying this Square
	Square[] neighbors;	// array of adjacent Squares within the Grid
	Vector3 worldCoordinates;	// 3D coordinates in Unity's world units
	Plane plane;	// the actual plane that this Square represents

	void Start() {

	}

	void Update() {

	}
}
