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
	public static bool conveyorSwitchable;
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
		plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		plane.transform.localScale = new Vector3(1.0f, 1.0f, .4f);
		plane.transform.Rotate(-90f, 0, 0);
		plane.renderer.material.mainTexture = Resources.Load("Textures/Tile2") as Texture;
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 5;
		Camera.main.backgroundColor = Color.white;
		string filename = EditorUtility.OpenFilePanel("Level file", "", "txt");
		floor = LevelLoader.LoadLevel(filename);
		//LevelWriter.WriteLevel(filename);
		//Debug.Log("Level Written.");
		LevelLoader.LoadLevel(filename);
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
		ObjectSelector.MakeObjectSelector(new Vector3(140.0f, Camera.main.pixelHeight - 50.0f, z), 1, 1,
						  Resources.Load("Textures/Tile2") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Wall, name: "Wall Selector");
	}

	void Update() {
		// if(Input.GetKeyDown("w")) {
		// 	Camera.main.transform.Translate(0, 1, 0);
		// }
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

				break;
			case ObjectType.SpikeFloor:

				break;
			case ObjectType.Paint:

				break;
			case ObjectType.Conveyor:

				break;
			case ObjectType.Player:
				
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
