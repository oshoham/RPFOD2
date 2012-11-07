using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour {
	
	public static readonly int WIDTH = 100;
	public static readonly int HEIGHT = 100;

	public static Grid floor;
	public static int level = 1;
	public static GameObject plane;

	void Start() {
		plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.transform.localScale = new Vector3(1.25f, 1.0f, .4f);
		plane.transform.Rotate(-90f, 0, 0);
		//plane.renderer.material.mainTexture = Resources.Load("Textures");
		
	}

}
