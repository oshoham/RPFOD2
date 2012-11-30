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

	public static float healthbar;
	
	public AudioSource bgm = new AudioSource();
	public AudioClip song;
	public AudioSource effects = new AudioSource();

	public Texture healthTexture;

	void Start() {
		Time.timeScale = 1;		
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 5;
		Camera.main.backgroundColor = Color.black;
		string audiofile;
		filename = GlobalSettings.currentFile; // this is so janky I feel embarassed writing this
		if(filename != "")
			floor = LevelLoader.LoadLevel(filename, out audiofile);
		else
			floor = LevelLoader.LoadLevel("fortesting.txt", out audiofile);
		if(audiofile == null)
			audiofile = "Audio/08 Sburban Jungle";
		GameObject light = new GameObject("Light");
		Light l = light.AddComponent<Light>();
		l.transform.position = player.transform.position;
		l.transform.Translate(0,0,-2);
		l.type = LightType.Point;
		l.transform.parent = player.transform;
		l.intensity = 0.4f;
		l.range = 100f;
		PlayerGui.MakePlayerGui(Color.red, new Vector3(130.0f, Camera.main.pixelHeight - 80.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(Color.green, new Vector3(190.0f, Camera.main.pixelHeight - 80.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(Color.blue, new Vector3(250.0f, Camera.main.pixelHeight - 80.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(player.defaultColor, new Vector3(310.0f, Camera.main.pixelHeight - 160.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.red, new Vector3(130.0f, Camera.main.pixelHeight - 160.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.green, new Vector3(190.0f, Camera.main.pixelHeight - 160.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.blue, new Vector3(250.0f, Camera.main.pixelHeight - 160.0f, Camera.main.nearClipPlane + 5.0f), false);
		GameObject light2 = new GameObject("Light");
		Light l2 = light2.AddComponent<Light>();
		l2.transform.position = new Vector3(-6.97F,4.52F,-16.8F);
		l2.transform.parent = GameObject.Find("Main Camera").transform;
		l2.type = LightType.Point;
		l2.intensity = 8f;
		l2.range = 3f;
		bgm = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
		song = Resources.Load("Audio/Effects/ambience3") as AudioClip;
		bgm.clip = song;
		bgm.loop = true;
		bgm.Play();
		healthTexture = Resources.Load("Textures/health") as Texture;
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
		//Gui style for ALIEN5 font 
		GUIStyle guiStyle = new GUIStyle();
		guiStyle.font = Resources.Load("Fonts/ALIEN5") as Font;
		guiStyle.normal.textColor = Color.white;
		guiStyle.fontSize = 23;
		guiStyle.fontStyle = FontStyle.Bold;
		GUIStyle healthgui = new GUIStyle();
		healthgui.font = Resources.Load("Fonts/ALIEN5") as Font;
		healthgui.normal.textColor = Color.white;
		healthgui.fontStyle = FontStyle.Bold;
		healthgui.fontSize = 12;

		if(GUI.Button(new Rect(20, 880, 150, 40), "Main Menu", guiStyle)) {                                   
                            Application.LoadLevel("StartScreen");                                                     
                }
		if(GlobalSettings.lastScene == "Editor") {
			if(GUI.Button(new Rect(20, 830, 150, 40), "Level Editor", guiStyle)) {
				GlobalSettings.lastScene = "Game";
				Application.LoadLevel("Editor");
			}
		}
		else if(GlobalSettings.lastScene == "FreePlaySelector") {
			if(GUI.Button(new Rect(20, 830, 150, 40), "Level Selector", guiStyle)) {
				GlobalSettings.lastScene = "Game";
				Application.LoadLevel("FreePlaySelector");
			}
		}
		if(GUI.Button(new Rect(20, 780, 150, 40), "Restart", guiStyle)) {
				Application.LoadLevel("Game");
		}
		if(player == null)
			return;

		//health bar
		//can't figure out how to change the color of the health bar without changing the color of all the rest of the gui
		//also the health bar width isn't behaving like it should
//		GUILayout.HorizontalScrollbar(0, GameManager.player.health, 0F, 15F, GUILayout.Height(1000), GUILayout.Width(1000));
//		GUI.DrawTexture(new Rect(10, 10, 300, 100), Resources.Load("Textures/PlayerReal") as Texture, ScaleMode.ScaleToFit, true, 0);
	        //healthbar = GUI.HorizontalScrollbar(new Rect(10, 10, 300, 10), 0, GameManager.player.health, 0, 15);
		GUI.DrawTexture(new Rect(10, 30, player.health < 0 ? 0 : player.health * 25, 10), healthTexture, ScaleMode.ScaleAndCrop);

		GUI.Label(new Rect(10, 10, 100, 50), "Health: " + player.health, guiStyle);
		GUI.Label(new Rect(10, 70, 100, 50), "Shooting:", guiStyle);
		GUI.Label(new Rect(10, 150, 100, 50), "Painted:", guiStyle);
		GUI.Label(new Rect(126, 70, 100, 20), "" + (player.colors.ContainsKey(Color.red) ? player.colors[Color.red] : 0), guiStyle);
		GUI.Label(new Rect(186, 70, 100, 20), "" + (player.colors.ContainsKey(Color.green) ? player.colors[Color.green] : 0), guiStyle);
		GUI.Label(new Rect(246, 70, 100, 20), "" + (player.colors.ContainsKey(Color.blue) ? player.colors[Color.blue] : 0), guiStyle);
//		GUI.Box(new Rect(1, 1, 320, 140), "");
		if(WinChecker.robotsWin) {
			GUI.Label(new Rect(10, 200, 200, 50), "Robot goal: " + WinChecker.robotLimit, guiStyle);
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
