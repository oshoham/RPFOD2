using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class ObjectSelector : MonoBehaviour {

	public Action onClick;
	public Vector3 position;
	
	void Update() {
		transform.position = Camera.main.ScreenToWorldPoint(position);
	}
	
	void OnMouseDown() {
		print("mouse DOWN");
		onClick();
	}
	
	public static GameObject MakeObjectSelector(Vector3 position, float width, float height, Texture texture,
						    Action onClick, string name = "Object Selector") {
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.name = name;
		plane.transform.Rotate(-90, 0, 0);
		plane.transform.localScale = new Vector3(width/10, 1, height/10);
		plane.renderer.material.mainTexture = texture;
		ObjectSelector script = plane.AddComponent<ObjectSelector>();
		script.onClick = onClick;
		script.position = position;
		return plane;
	}
}
