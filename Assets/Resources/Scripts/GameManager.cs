using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 40;
	public static readonly int HEIGHT = 40;

	public static Grid floor;
	public static Player player;

	public static string filename;

	public static int level = 1;
//	public static GameObject plane;

	void Start() {
//		plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
//		plane.transform.localScale = new Vector3(2.0f, 1.0f, 1.0f);
//		plane.transform.Rotate(-90, 0, 0);
//	     	plane.renderer.material.mainTexture = Resources.Load("Textures/Tile2") as Texture;
		Time.timeScale = 1;
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 5;
		Camera.main.backgroundColor = Color.black;
		filename = GlobalSettings.currentFile; // this is so janky I feel embarassed writing this
		if(filename != "")
			floor = LevelLoader.LoadLevel(filename);
		else
			floor = LevelLoader.LoadLevel("fortesting.txt");
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
	}

	/*
	 * Should be called when win conditions are met.
	 */
	public static void Win() {
		print("A winner is you!");
		Application.LoadLevel("WinScreen");
	}
	
	void OnGUI() {
		if(GUI.Button(new Rect(10, 540, 150, 40), "Main Menu")) {                                   
                            Application.LoadLevel("StartScreen");                                                     
                }
		if(GlobalSettings.lastScene == "Editor") {
			if(GUI.Button(new Rect(10, 490, 150, 40), "Level Editor")) {
				GlobalSettings.lastScene = "Game";
				Application.LoadLevel("Editor");
			}
		}
		else if(GlobalSettings.lastScene == "FreePlaySelector") {
			if(GUI.Button(new Rect(10, 490, 150, 40), "Level Selector")) {
				GlobalSettings.lastScene = "Game";
				Application.LoadLevel("FreePlaySelector");
			}
		}
		if(GUI.Button(new Rect(10, 440, 150, 40), "Restart")) {
				Application.LoadLevel("Game");
		}
		if(player == null)
			return;
		//Gui style for ALIEN5 font 
		GUIStyle guiStyle = new GUIStyle();
		guiStyle.font = Resources.Load("Fonts/ALIEN5") as Font;
		guiStyle.normal.textColor = Color.white;
		guiStyle.fontSize = 23;
		guiStyle.fontStyle = FontStyle.Bold;

		GUI.Label(new Rect(10, 10, 100, 50), "Health: " + player.health, guiStyle);
		GUI.Label(new Rect(10, 40, 100, 50), "Shooting:", guiStyle);
		GUI.Label(new Rect(10, 100, 100, 50), "Painted:", guiStyle);
		GUI.Label(new Rect(135, 40, 100, 20), "" + (player.colors.ContainsKey(Color.red) ? player.colors[Color.red] : 0), guiStyle);
		GUI.Label(new Rect(185, 40, 100, 20), "" + (player.colors.ContainsKey(Color.green) ? player.colors[Color.green] : 0), guiStyle);
		GUI.Label(new Rect(235, 40, 100, 20), "" + (player.colors.ContainsKey(Color.blue) ? player.colors[Color.blue] : 0), guiStyle);
//		GUI.Box(new Rect(1, 1, 320, 140), "");
		if(WinChecker.robotsWin) {
			GUI.Label(new Rect(10, 135, 200, 50), "Robot goal: " + WinChecker.robotLimit, guiStyle);
		}
	}
	
	/*
	 * Move a GameObject mover from start to end. Returns true if the object
	 * actually ended up moving, false otherwise.
	 */
	public static bool Move(Vector2 start, Vector2 end, GameObject mover) {
		if(floor != null && !floor.Check(end)) {
			floor.Move(start, end, mover);
			return true;
		}
		return false;
	}
}
