using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 40;
	public static readonly int HEIGHT = 40;

	public static Grid floor;
	public static Player player;

	public static int level = 1;
	public static GameObject plane; // take this out!

	void Start() {
		plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.transform.localScale = new Vector3(1.25f, 1.0f, .4f);
		plane.transform.Rotate(-90, 0, 0);
	     	plane.renderer.material.mainTexture = Resources.Load("Textures/Tile2") as Texture;
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 8;
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
		PlayerGui.MakePlayerGui(Color.red, new Vector3(140.0f, Camera.main.pixelHeight - 50.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(Color.green, new Vector3(190.0f, Camera.main.pixelHeight - 50.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(Color.blue, new Vector3(240.0f, Camera.main.pixelHeight - 50.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(player.defaultColor, new Vector3(290.0f, Camera.main.pixelHeight - 110.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.red, new Vector3(140.0f, Camera.main.pixelHeight - 110.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.green, new Vector3(190.0f, Camera.main.pixelHeight - 110.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.blue, new Vector3(240.0f, Camera.main.pixelHeight - 110.0f, Camera.main.nearClipPlane + 5.0f), false);
	}

	void Update() {

		plane.renderer.material.color = Color.white;
		if(Input.GetKeyDown("p")) {
			print("Woo!");
			LevelWriter.WriteLevel("fizz.txt");
		}
	}

	void OnGUI() {
		if(player == null)
			return;
		GUIStyle guiStyle = new GUIStyle();
		guiStyle.font = Resources.Load("Fonts/Chalkduster") as Font;
		GUI.Label(new Rect(10, 10, 100, 50), "Health: " + player.health, guiStyle);
		GUI.Label(new Rect(10, 40, 100, 50), "Shooting:", guiStyle);
		GUI.Label(new Rect(10, 100, 100, 50), "Painted:", guiStyle);
		GUI.Label(new Rect(135, 70, 20, 20), "" + (player.colors.ContainsKey(Color.red) ? player.colors[Color.red] : 0), guiStyle);
		GUI.Label(new Rect(185, 70, 20, 20), "" + (player.colors.ContainsKey(Color.green) ? player.colors[Color.green] : 0), guiStyle);
		GUI.Label(new Rect(235, 70, 20, 20), "" + (player.colors.ContainsKey(Color.blue) ? player.colors[Color.blue] : 0), guiStyle);
	}
	
	/*
	 * Move a GameObject mover from start to end. Returns true if the object
	 * actually ended up moving, false otherwise.
	 */
	public static bool Move(Vector2 start, Vector2 end, GameObject mover) {
		if(!floor.Check(end)) {
			floor.Move(start, end, mover);
			return true;
		}
	return false;
	}

	public static void makeLev(int lev)
	{
		switch(lev){
			case 1:
				new L1();
				break;
			default:
				break;
		}
	}	
}
