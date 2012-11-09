using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 40;
	public static readonly int HEIGHT = 40;

	public static Grid floor;
	public static Player player;

	public static int level = 1;
//	public static GameObject plane;

	void Start() {
//		plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
//		plane.transform.localScale = new Vector3(2.0f, 1.0f, 1.0f);
//		plane.transform.Rotate(-90, 0, 0);
//	     	plane.renderer.material.mainTexture = Resources.Load("Textures/Tile2") as Texture;
		Time.timeScale = 1;
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 8;
		Camera.main.backgroundColor = Color.white;
		string filename = "L2.txt";//EditorUtility.OpenFilePanel("Level file", "", "txt");
		floor = LevelLoader.LoadLevel(filename);
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

//		plane.renderer.material.ycolor = Color.white;
		if(Input.GetKeyDown("p")) {
			print("Woo!");
			LevelWriter.WriteLevel("fizz.txt", floor);
		}
	}

	void OnGUI() {
		if(player == null)
			return;
		GUIStyle guiStyle = new GUIStyle();
		guiStyle.font = Resources.Load("Fonts/Chalkduster") as Font;
/* 
   GUI Box that doesn't work the way I want it to
		GUIStyle boxStyle = new GUIStyle();
		Texture2D texture = new Texture2D(256, 256);
	        for (int y = 0; y < texture.height; ++y) {
	            for (int x = 0; x < texture.width; ++x) {
        	        Color color = new Color(1f, 1f, 1f);
                	texture.SetPixel(x, y, color);
            	    }
               }
	       for (int y = 0; y< texture.height; ++y) {
	       	   Color color = new Color(0f, 0f, 0f);
		   texture.SetPixel(0, y, color);
		   texture.SetPixel(texture.width, y, color);
	       }
       	       texture.Apply();
               boxStyle.normal.background = texture; 
		GUI.Box(new Rect(1, 1, 320, 140), new GUIContent(""), boxStyle);
*/		
		GUI.Label(new Rect(10, 10, 100, 50), "Health: " + player.health, guiStyle);
		GUI.Label(new Rect(10, 40, 100, 50), "Shooting:", guiStyle);
		GUI.Label(new Rect(10, 100, 100, 50), "Painted:", guiStyle);
		GUI.Label(new Rect(135, 70, 20, 20), "" + (player.colors.ContainsKey(Color.red) ? player.colors[Color.red] : 0), guiStyle);
		GUI.Label(new Rect(185, 70, 20, 20), "" + (player.colors.ContainsKey(Color.green) ? player.colors[Color.green] : 0), guiStyle);
		GUI.Label(new Rect(235, 70, 20, 20), "" + (player.colors.ContainsKey(Color.blue) ? player.colors[Color.blue] : 0), guiStyle);
                if(GUI.Button(new Rect(10, 860, 150, 40), "Main Menu")) {                                          
                            Application.LoadLevel("StartScreen");                                                     
                } 
//		GUI.Box(new Rect(1, 1, 320, 140), "");
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

	// public static void makeLev(int lev)
	// {
	// 	switch(lev){
	// 		case 1:
	// 			new L1();
	// 			break;
	// 		default:
	// 			break;
	// 	}
	// }
}
