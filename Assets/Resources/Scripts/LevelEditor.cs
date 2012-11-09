using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ObjectType {
	Wall,
	SpikeWall,
	SpikeFloor,
	Paint,
	Conveyor,
	Player,
	Robot
}

public class LevelEditor : MonoBehaviour {
	
	public static int WIDTH = 100;
	public static int HEIGHT = 100;

	public static Grid floor;
	public static int level = 1;
	public static GameObject plane;

	public static ObjectType objectToBeCreated;
	
	/*
	 * Information for each type of object that we might create. Most of
	 * these aren't explicitly initialized because the default values for
	 * each type (0, null, false, Color.black, etc.) are good enough.
	 *
	 * Global variables: AWESOME programming style!
	 */
	
	// Wall
	public static int wallHealth = 10;
	public static bool wallDestructible;
	public static Color wallColor = Color.white;

	// SpikeWall
	public static int spikeWallHealth = 10;
	public static bool spikeWallDestructible;
	public static List<Vector2> spikeWallDirections = new List<Vector2>();
	public static Color spikeWallColor = Color.white;

	// Paint
	public static Color paintColor = Color.red;
	public static float paintRespawnTime;

	// Conveyor
	public static Vector2 conveyorDirection = new Vector2(1, 0);
	public static float conveyorLength = 1;
	public static float conveyorSpeed = 0.5f;
	public static bool conveyorSwitchable = false;
	public static float conveyorSwitchRate = 1.0f;

	// Player
	public static int playerHealth = 15;

	// Robot
	public static float robotSpeed = 0.5f;
	public static int robotDamageDealt = 1;
	public static int robotHealth = 10;
	public static int robotForwardRange = 1;
	public static int robotSideRange = 1;
	public static Vector2 robotMovementDirection = new Vector2(1, 0);
	public static Color robotColorVisible = Color.red;
	public static Vector2 robotFireDirection = new Vector2(1, 0);
	public static bool robotTurnsLeft;
	
	void Start() {
		Time.timeScale = 0;
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 5;
		Camera.main.backgroundColor = Color.white;
		string filename = EditorUtility.OpenFilePanel("Level file", "", "txt");
		floor = LevelLoader.LoadLevel(filename);
		//LevelWriter.WriteLevel(filename);
		//Debug.Log("Level Written.");
		GameObject light = new GameObject("Light");
		Light l = light.AddComponent<Light>();
		light.transform.position = Camera.main.transform.position;
		l.type = LightType.Directional;
		l.intensity = 0.4f;
		// Set up object placers
		for(int i = 0; i < floor.grid.GetLength(0); i++) {
			for(int j = 0; j < floor.grid.GetLength(1); j++) {
				ObjectPlacer.MakeObjectPlacer(i, j, floor);
			}
		}
		// Set up object selectors
		float z = Camera.main.nearClipPlane + 5;
		ObjectSelector.MakeObjectSelector(new Vector3(100.0f, Camera.main.pixelHeight - 50.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/Wall") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Wall, name: "Wall Selector");
//Commented out SpikeWall because there's no difference between Spike Wall and Spike Floor, effectively. Check the Design Doc for more info
//		ObjectSelector.MakeObjectSelector(new Vector3(100.0f, Camera.main.pixelHeight - 90.0f, z), 0.5f, 0.5f,
//						  Resources.Load("Textures/Spike") as Texture,
//						  () => LevelEditor.objectToBeCreated = ObjectType.SpikeWall, name: "SpikeWall Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(100.0f, Camera.main.pixelHeight - 100.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/Electrocute") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.SpikeFloor, name: "SpikeFloor Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(100.0f, Camera.main.pixelHeight - 150.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/Paint") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Paint, name: "Paint Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(100.0f, Camera.main.pixelHeight - 200.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/Conveyor") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Conveyor, name: "Conveyor Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(100.0f, Camera.main.pixelHeight - 250.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/PlayerPacMan") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Player, name: "Player Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(100.0f, Camera.main.pixelHeight - 300.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/BlankBot") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Robot, name: "Robot Selector");
	}

	void Update() {
		if(Input.GetKeyDown("w")) {
			Camera.main.transform.Translate(0, 1, 0);
		}
		if(Input.GetKeyDown("d")) {
			Camera.main.transform.Translate(1, 0, 0);
		}
		if(Input.GetKeyDown("s")) {
			Camera.main.transform.Translate(0, -1, 0);
		}
		if(Input.GetKeyDown("a")) {
			Camera.main.transform.Translate(-1, 0, 0);
		}
	}
	
	void OnGUI() {
		switch(objectToBeCreated) {
			case ObjectType.Wall:
				// health
				try {
					wallHealth = Int32.Parse(GUI.TextField(FromBottomRight(new Rect(300, 50, 50, 10)),
									       "" + wallHealth));
				}
				catch {
					Debug.Log("Wrong number format!");
				}
				// destructible
				wallDestructible = GUI.Toggle(FromBottomRight(new Rect(300, 70, 50, 10)),
							      wallDestructible,
							      "Destructible?");
				// color
				int colorInt;
				if(wallColor == Color.red) {
					colorInt = 0;
				}
				else if(wallColor == Color.green) {
					colorInt = 1;
				}
				else if(wallColor == Color.blue) {
					colorInt = 2;
				}
				else {
					colorInt = 3;
				}
				colorInt = GUI.Toolbar(FromBottomRight(new Rect(300, 90, 250, 30)),
						       colorInt,
						       new string[] {"Red", "Green", "Blue", "None"});
				switch(colorInt) {
					case 0:
						wallColor = Color.red;
						break;
					case 1:
						wallColor = Color.green;
						break;
					case 2:
						wallColor = Color.blue;
						break;
					case 3:
						wallColor = Color.white;
						break;
				}
				break;
			case ObjectType.SpikeWall:
				// health 
				try {
					GUI.Label(FromBottomRight(new Rect(300, 50, 50, 10)), "Health");
					spikeWallHealth = Int32.Parse(GUI.TextField(FromBottomRight(new Rect(300, 50, 50, 10)),
										 "" + spikeWallHealth));
				}
				catch {
					Debug.Log("Wrong number format!");
				}
				// destructible
				spikeWallDestructible = GUI.Toggle(FromBottomRight(new Rect(300, 70, 50, 10)),
								spikeWallDestructible, "Destructible?");	
				// color
				if(spikeWallColor == Color.red) {
					colorInt = 0;
				}
				else if(spikeWallColor == Color.green) {
					colorInt = 1;
				}
				else if(spikeWallColor == Color.blue) {
					colorInt = 2;
				}
				else {
					colorInt = 3;
				}
				colorInt = GUI.Toolbar(FromBottomRight(new Rect(300, 90, 250, 30)),
						       colorInt,
						       new string[] {"Red", "Green", "Blue", "None"});
				switch(colorInt) {
					case 0:
						spikeWallColor = Color.red;
						break;
					case 1:
						spikeWallColor = Color.green;
						break;
					case 2:
						spikeWallColor = Color.blue;
						break;
					case 3:
						spikeWallColor = Color.white;
						break;
				}
				break;
			case ObjectType.SpikeFloor:

				break;
			case ObjectType.Paint:
				// spawn
				try {
					GUI.Label(FromBottomRight(new Rect(300, 50, 50, 10)), "Respawn Rate");
					paintRespawnTime = Single.Parse(GUI.TextField(FromBottomRight(new Rect(300, 50, 50, 10)), "" + paintRespawnTime)); 
				}	
				catch {
					Debug.Log("Wrong number format!");
				}
				// color
				if(paintColor == Color.red) {
					colorInt = 0;
				}
				else if(paintColor == Color.green) {
					colorInt = 1;
				}
				else if(paintColor == Color.blue) {
					colorInt = 2;
				}
				else {
					colorInt = 3;
				}
				colorInt = GUI.Toolbar(FromBottomRight(new Rect(300, 90, 250, 30)),
						       colorInt,
						       new string[] {"Red", "Green", "Blue", "None"});
				switch(colorInt) {
					case 0:
						paintColor = Color.red;
						break;
					case 1:
						paintColor = Color.green;
						break;
					case 2:
						paintColor = Color.blue;
						break;
					case 3:
						paintColor = Color.white;
						break;
				}
				break;
			case ObjectType.Conveyor:

				break;
			case ObjectType.Player:	
				try {
					GUI.Label(FromBottomRight(new Rect(300, 50, 50, 10)), "Health");
					playerHealth = Int32.Parse(GUI.TextField(FromBottomRight(new Rect(300, 50, 50, 10)), "" + paintRespawnTime)); 
				}
				catch {
					Debug.Log("Wrong number format!");
				}
	
				break;	
			case ObjectType.Robot:

				break;
		}
	}
	
	/*
	 * Returns the same Rect but offset from the bottom right instead
	 * of the top left. Useful for GUI stuff.
	 */
	public static Rect FromBottomRight(Rect r) {
		return new Rect(Camera.main.pixelWidth - r.x,
				Camera.main.pixelHeight - r.y,
				r.width,
				r.height);
	}
}
