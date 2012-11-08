using UnityEngine;
using UnityEditor;
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
	public static float playerHealth = 15;

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
		
	}

	void Update() {}
}
