using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LevelEditor : MonoBehaviour {
	
	public static int WIDTH = 100;
	public static int HEIGHT = 100;

	public static Grid floor;
	public static int level = 1;
	public static GameObject plane;

	public static string objectToBeCreated;

	void Start() {
		plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.transform.localScale = new Vector3(, 1.0f, .4f);
		plane.transform.Rotate(-90f, 0, 0);
		plane.renderer.material.mainTexture = Resources.Load("Textures/Tile2") as Texture;
		Camera.main.orthographic = true;
		Camera.main.backgroundColor = Color.white;
		string filename = EditorUtility.OpenFilePanel("Level file", "", "txt");
		LevelLoader.LoadLevel(filename);
		LevelWriter.WriteLevel(filename);
		Debug.Log("Level Written.");
		LevelLoader.LoadLevel(filename);
		GameObject light = new GameObject("Light");
		Light l = light.AddComponent<Light>();
		light.transform.position = Camera.main.transform.position;
		l.type = LightType.Directional;
		l.intensity = 0.4f;
		
	}

	void Update() {}
}
