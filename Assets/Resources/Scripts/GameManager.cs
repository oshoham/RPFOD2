using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour {

	public static readonly int WIDTH = 40;
	public static readonly int HEIGHT = 40;

	public static Grid floor;
	public static Player player;

	public static string filename;

	public static int level = 1;

	public static float healthbar;
	
	public AudioSource bgm = new AudioSource();
	public AudioClip song;
	public AudioSource effects = new AudioSource();
	
	public string textfile;

	public static int tutorialLevel;

	public Texture healthTexture;

	void Start() {
		Time.timeScale = 1;
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 5;
		Camera.main.backgroundColor = Color.black;
		string audiofile;
		filename = GlobalSettings.currentFile; // this is so janky I feel embarassed writing this
		if(filename != "")
			floor = LevelLoader.LoadLevel(filename, out audiofile, out textfile);
		else
			floor = LevelLoader.LoadLevel("fortesting.txt", out audiofile, out textfile);
		// Light for the player.
		GameObject light = new GameObject("Light");
		Light l = light.AddComponent<Light>();
		l.transform.position = player.transform.position;
		l.transform.Translate(0,0,-2);
		l.type = LightType.Point;
		l.transform.parent = player.transform;
		l.intensity = .5f;
		l.range = 100f;
		// Light for GUI.
		GameObject light2 = new GameObject("Light");
		Light l2 = light2.AddComponent<Light>();
		l2.transform.position = new Vector3(-6.97F,4.52F,-16.8F);
		l2.transform.parent = GameObject.Find("Main Camera").transform;
		l2.type = LightType.Point;
		l2.intensity = 8f;
		l2.range = 3f;
		// GameObject light2 = new GameObject("Light");
		// Light l2 = light2.AddComponent<Light>();
		// l2.transform.position = new Vector3(.5f, 0, -1);
		// l2.type = LightType.Point;
		// l2.intensity = 8f;
		// l2.range = 3f;
		PlayerGui.MakePlayerGui(Color.red, new Vector3(130.0f, Camera.main.pixelHeight - 80.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(Color.green, new Vector3(190.0f, Camera.main.pixelHeight - 80.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(Color.blue, new Vector3(250.0f, Camera.main.pixelHeight - 80.0f, Camera.main.nearClipPlane + 5.0f), true);
		PlayerGui.MakePlayerGui(player.defaultColor, new Vector3(310.0f, Camera.main.pixelHeight - 160.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.red, new Vector3(130.0f, Camera.main.pixelHeight - 160.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.green, new Vector3(190.0f, Camera.main.pixelHeight - 160.0f, Camera.main.nearClipPlane + 5.0f), false);
		PlayerGui.MakePlayerGui(Color.blue, new Vector3(250.0f, Camera.main.pixelHeight - 160.0f, Camera.main.nearClipPlane + 5.0f), false);
	

		bgm = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
		song = Resources.Load("Audio/" + audiofile) as AudioClip;
		bgm.clip = song;
		bgm.loop = true;
		bgm.Play();
		healthTexture = Resources.Load("Textures/health") as Texture;

	}	

	void Update(){
		if(Input.GetKey("r")) {
			Application.LoadLevel("Game");
		}
	}

	/*
	 * Should be called when win conditions are met.
	 */
	public static void Win() {
		if(Application.loadedLevelName == "Tutorial") {
			tutorialLevel++;
			if(tutorialLevel > 4) {
				tutorialLevel = 1;
				Application.LoadLevel("WinScreen");
			}
			else {
				GlobalSettings.currentFile = "L" + tutorialLevel + ".txt";
				Application.LoadLevel("Tutorial");
			}
			return;
		}
		else if(GlobalSettings.lastScene == "CavalcadeMap") {
			string path = Path.Combine(Application.dataPath, "SaveFile.txt");
			if(File.Exists(path)) {
				StreamReader reader = new StreamReader(path);
				int levelsCompleted = Int32.Parse(reader.ReadLine());
				reader.Close();
				int levelNumber = GetLevelNumber(filename);
				if(levelNumber > levelsCompleted) {
					using(StreamWriter writer = File.CreateText(path)) {
						writer.WriteLine(levelNumber);
						writer.Close();
						Debug.Log("Current Progress: " + levelNumber + " levels.");
					}
				}
			}
		}
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

		if(GUI.Button(new Rect(20, Camera.main.pixelHeight - 150, 150, 40), "Main Menu", guiStyle)) {                                   
                            Application.LoadLevel("StartScreen");                                                     
                }
		if(GlobalSettings.lastScene == "Editor") {
			if(GUI.Button(new Rect(20, Camera.main.pixelHeight - 200, 150, 40), "Level Editor", guiStyle)) {
				GlobalSettings.lastScene = "Game";
				Application.LoadLevel("Editor");
			}
		}
		else if(GlobalSettings.lastScene == "FreePlaySelector") {
			if(GUI.Button(new Rect(20, Camera.main.pixelHeight - 50, 150, 40), "Level Selector", guiStyle)) {
				GlobalSettings.lastScene = "Game";
				Application.LoadLevel("FreePlaySelector");
			}
		}
		else if(GlobalSettings.lastScene == "CavalcadeMap") {
			if(GUI.Button(new Rect(20, Camera.main.pixelHeight - 50, 150, 40), "Cavalcade Map", guiStyle)) {
				GlobalSettings.lastScene = "Game";
				Application.LoadLevel("CavalcadeMap");
			}
		}
		if(GUI.Button(new Rect(20, Camera.main.pixelHeight - 100, 150, 40), "Restart", guiStyle)) {
				Application.LoadLevel("Game");
		}
		if(player == null)
			return;
		
		// health bar
		GUI.DrawTexture(new Rect(10, 30, player.health < 0 ? 0 : player.health * 25, 10), healthTexture, ScaleMode.ScaleAndCrop);

		GUI.Label(new Rect(100, 500, 1000, 500), textfile, guiStyle);

		GUI.Label(new Rect(10, 10, 100, 50), "Health: " + player.health, guiStyle);
		GUI.Label(new Rect(10, 70, 100, 50), "Shooting:", guiStyle);
		GUI.Label(new Rect(10, 150, 100, 50), "Painted:", guiStyle);
		GUI.Label(new Rect(126, 70, 100, 20), "" + (player.colors.ContainsKey(Color.red) ? player.colors[Color.red] : 0), guiStyle);
		GUI.Label(new Rect(186, 70, 100, 20), "" + (player.colors.ContainsKey(Color.green) ? player.colors[Color.green] : 0), guiStyle);
		GUI.Label(new Rect(246, 70, 100, 20), "" + (player.colors.ContainsKey(Color.blue) ? player.colors[Color.blue] : 0), guiStyle);
		if(WinChecker.robotsWin) {
			GUI.Label(new Rect(10, 200, 200, 50), "Robot goal: " + WinChecker.robotLimit, guiStyle);
			GUI.Label(new Rect(10, 250, 200, 50), "Robots left: " + WinChecker.numRobots, guiStyle);
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

	public static int GetLevelNumber(string filename) {
		if(filename != "") {
			 filename = Path.GetFileNameWithoutExtension(filename);
			 if(filename == "L5")
				 return 1;
			 else if(filename == "L6")
				 return 2;
			 else if(filename == "KeepAlive")
				 return 3;
			 else if(filename == "KeepAlive2")
				 return 4;
			 else if(filename == "Covertop")
				 return 5;
			 else if(filename == "Whirlpool")
				 return 6;
			 else if(filename == "KillAllHumans")
				 return 7;
			 else if(filename == "Virus")
				 return 8;
			 else if(filename == "RGB")
				 return 9;
			 else if(filename == "Olympus")
				 return 10;
			 else
				 return -1;
		}
		else
			return -1;
	}
}
