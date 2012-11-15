using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

//Script for handling the levelEditor 

public enum ObjectType {
	Wall,
	SpikeWall,
	SpikeFloor,
	Paint,
	Conveyor,
	Player,
	Robot,
	DestructibleWall,
	ExplosiveCrate
}

public class LevelEditor : MonoBehaviour {
	
	public static int WIDTH = 100;
	public static int HEIGHT = 100;

	public static Grid floor;
	public static int level = 1;
	public static GameObject plane;

	public static ObjectType objectToBeCreated;
	public static String saveFileName = "";
	public static String loadFileName = "";
	public static string widthCommaHeight = "";
	public static int newWidth;
	public static int newHeight;

	public static List<GameObject> objectPlacers = new List<GameObject>();
	
	/*
	 * Information for each type of object that we might create. Most of
	 * these aren't explicitly initialized because the default values for
	 * each type (0, null, false, Color.black, etc.) are good enough.
	 *
	 * Global variables: AWESOME programming style!
	 */
	
	// Wall
	public static Color wallColor = Color.white;

	// SpikeWall
	public static string spikeWallHealth = "10";
	public static bool spikeWallDestructible;
	public static List<Vector2> spikeWallDirections = new List<Vector2>();
	public static Color spikeWallColor = Color.white;

	// Paint
	public static Color paintColor = Color.red;
	public static string paintRespawnTime = "0";

	// Conveyor
	public static Vector2 conveyorDirection = new Vector2(1, 0);
	public static string conveyorLength = "1";
	public static string conveyorSpeed = "0.5";
	public static bool conveyorSwitchable = false;
	public static string conveyorSwitchRate = "1";

	// Player
	public static string playerHealth = "15";

	// Robot
	public static string robotSpeed = "0.5";
	public static string robotDamageDealt = "1";
	public static string robotHealth = "10";
	public static string robotForwardRange = "1";
	public static string robotSideRange = "1";
	public static Vector2 robotMovementDirection = new Vector2(1, 0);
	public static Color robotColorVisible = Color.red;
	public static Vector2 robotFireDirection = new Vector2(1, 0);
	public static RotationMatrix robotRotation;

	// DestructibleWall
	public static string destructibleWallHealth = "10";
	public static Color destructibleWallColor;

	// Win conditions
	public static bool robotsWin = false;
	public static string robotLimit = "-1";
	public static bool squareWins = false;
	public static string winCoords = "";
	
	void Start() {
		Time.timeScale = 0;
		Camera.main.orthographic = true;
		Camera.main.orthographicSize = 5;
		Camera.main.backgroundColor = Color.white;
		//string filename = EditorUtility.OpenFilePanel("Level file", "", "txt");
		//floor = LevelLoader.LoadLevel(filename);
		GameObject light = new GameObject("Light");
		Light l = light.AddComponent<Light>();
		light.transform.position = Camera.main.transform.position;
		l.type = LightType.Directional;
		l.intensity = 0.4f;
		// Set up object selectors
		float z = Camera.main.nearClipPlane + 5;
		ObjectSelector.MakeObjectSelector(new Vector3(50.0f, Camera.main.pixelHeight - 200.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/WallIcon") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Wall, name: "Wall Selector");
//Commented out SpikeWall because there's no difference between Spike Wall and Spike Floor, effectively. Check the Design Doc for more info
//		ObjectSelector.MakeObjectSelector(new Vector3(50.0f, Camera.main.pixelHeight - 140.0f, z), 0.5f, 0.5f,
//						  Resources.Load("Textures/Spike") as Texture,
//						  () => LevelEditor.objectToBeCreated = ObjectType.SpikeWall, name: "SpikeWall Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(50.0f, Camera.main.pixelHeight - 250.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/ElectrocuteIcon") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.SpikeFloor, name: "SpikeFloor Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(50.0f, Camera.main.pixelHeight - 300.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/PaintIcon") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Paint, name: "Paint Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(50.0f, Camera.main.pixelHeight - 350.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/ConveyorIcon") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Conveyor, name: "Conveyor Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(50.0f, Camera.main.pixelHeight - 400.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/PlayerIcon") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Player, name: "Player Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(50.0f, Camera.main.pixelHeight - 450.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/BotIcon") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.Robot, name: "Robot Selector");
		ObjectSelector.MakeObjectSelector(new Vector3(50.0f, Camera.main.pixelHeight - 500.0f, z), 0.5f, 0.5f,
						  Resources.Load("Textures/WallIcon") as Texture,
						  () => LevelEditor.objectToBeCreated = ObjectType.DestructibleWall, name: "DestructibleWall Selector");

		if(GlobalSettings.lastScene == "Game")
			LevelLoader.LoadLevel(GlobalSettings.currentFile);
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
		saveFileName = GUI.TextField(new Rect(10, 10, 100, 20), saveFileName);
		if(GUI.Button(new Rect(120, 10, 50, 20), "Save")) {
			if(!robotsWin && !squareWins) {
				print("Hey bro! Might want some way to win the level, eh?");
			}
			else {
				LevelWriter.WriteLevel(saveFileName, floor);
				GlobalSettings.currentFile = saveFileName;
			}
		}
		loadFileName = GUI.TextField(new Rect(10, 50, 100, 20), loadFileName);
		if(GUI.Button(new Rect(120, 50, 50, 20), "Load")) {
			if(floor != null) {
				floor.Clear();
			}
			Grid newGrid = LevelLoader.LoadLevel(loadFileName);
			if(newGrid != null) {
				floor = newGrid;
				SetupObjectPlacers();
				newWidth = floor.grid.GetLength(0);
				newHeight = floor.grid.GetLength(1);
			}
			GlobalSettings.currentFile = loadFileName;
		}
		widthCommaHeight = GUI.TextField(new Rect(10, 90, 100, 20), widthCommaHeight);
		if(GUI.Button(new Rect(120, 90, 100, 20), "Resize")) {
			try {
				string[] bits = widthCommaHeight.Split(new char[] {','});
				floor = floor.Copy(Int32.Parse(bits[0]), Int32.Parse(bits[1]));
				try {
					SetupObjectPlacers();
				}
				catch {
					print("haha, objects");
				}
			}
			catch (Exception e) {
				print("shit! " + e.StackTrace);
			}
		}
		if(GUI.Button(new Rect(10, 120, 100, 20), "Reset")) {
			if(floor == null) {
				floor = new Grid(newWidth == 0 ? 10 : newWidth,
						 newHeight == 0 ? 10 : newHeight);
				SetupObjectPlacers();
			}
			else {
				floor.ClearObjects();
			}
		}
		if(GUI.Button(new Rect(120, 120, 100, 20), "Play")) {
			if(GlobalSettings.currentFile != "") {
				Application.LoadLevel("Game");
				GlobalSettings.lastScene = "Game";
			}
			else
				Debug.Log("You should probably load a level first or something.");
		}
		if(GUI.Button(new Rect(10, 860, 150, 40), "Main Menu")) {
			Application.LoadLevel("StartScreen");	  
		}
		// Win conditions
		robotsWin = GUI.Toggle(new Rect(300, 120, 20, 20),  robotsWin, "Robots win");
		robotLimit = GUI.TextField(new Rect(350, 120, 100, 20), robotLimit);
		squareWins = GUI.Toggle(new Rect(300, 150, 20, 20), squareWins, "Square wins (x, y)");
		winCoords = GUI.TextField(new Rect(350, 150, 100, 20), winCoords);
		// Object-specific stuffs
		switch(objectToBeCreated) {
			case ObjectType.Wall:
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
				colorInt = GUI.Toolbar(FromBottomRight(new Rect(300, 120, 250, 30)),
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
				GUI.Label(FromBottomRight(new Rect(300, 50, 50, 10)), "Health");
				spikeWallHealth = GUI.TextField(FromBottomRight(new Rect(300, 50, 50, 10)), spikeWallHealth);
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
			case ObjectType.Paint:
				// spawn
				GUI.Label(FromBottomRight(new Rect(250, 50, 50, 20)), "Respawn Rate");
				paintRespawnTime = GUI.TextField(FromBottomRight(new Rect(300, 50, 50, 20)), paintRespawnTime);
				Debug.Log("Wrong number format!");
				// color
				if(paintColor == Color.red) {
					colorInt = 0;
				}
				else if(paintColor == Color.green) {
					colorInt = 1;
				}
				else {
					colorInt = 2;
				}
				colorInt = GUI.Toolbar(FromBottomRight(new Rect(300, 90, 250, 30)),
						       colorInt,
						       new string[] {"Red", "Green", "Blue"});
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
				}
				break;
			case ObjectType.Conveyor:
				// length
				try {
					conveyorLength = GUI.TextField(FromBottomRight(new Rect(500, 50, 175, 30)), "" + conveyorLength);
					conveyorSpeed = GUI.TextField(FromBottomRight(new Rect(500, 30, 175, 30)), "" + conveyorSpeed);
					conveyorSwitchable = GUI.Toggle(FromBottomRight(new Rect(300, 50, 175, 30)), conveyorSwitchable, "Switchable");
					conveyorSwitchRate = GUI.TextField(FromBottomRight(new Rect(300, 30, 175, 30)), "" + conveyorSwitchRate);
				}
				catch {
					
				}
				// direction
				int direct;
				if(conveyorDirection.y > 0) {
					direct = 0;
				}
				else if(conveyorDirection.x > 0) {
					direct = 1;
				}
				else if(conveyorDirection.y < 0) {
					direct = 2;
				}
				else {
					direct = 3;
				}
				direct = GUI.Toolbar(FromBottomRight(new Rect(500, 90, 175, 30)),
						       direct,
						       new string[] {"North", "East", "South", "West"});
				switch(direct) {
					case 0:
						conveyorDirection = new Vector2(0, 1);
						break;
					case 1:
						conveyorDirection = new Vector2(1, 0);
						break;
					case 2:
						conveyorDirection = new Vector2(0, -1);
						break;
					case 3:
						conveyorDirection = new Vector2(-1, 0);
						break;
				}
				break;
			case ObjectType.Player:	
				GUI.Label(FromBottomRight(new Rect(300, 50, 50, 10)), "Health");
				playerHealth = GUI.TextField(FromBottomRight(new Rect(300, 50, 100, 50)), "" + playerHealth);
				break;	
			case ObjectType.Robot:
				GUI.Label(FromBottomRight(new Rect(375, 150, 75, 50)), "Health");
				GUI.Label(FromBottomRight(new Rect(300, 150, 75, 50)), "ForwardRange");
				GUI.Label(FromBottomRight(new Rect(225, 150, 75, 50)), "SideRange");
				GUI.Label(FromBottomRight(new Rect(150, 150, 75, 50)), "Speed");
				
				robotHealth = GUI.TextField(FromBottomRight(new Rect(375, 50, 50, 25)), robotHealth);
				robotForwardRange = GUI.TextField(FromBottomRight(new Rect(300, 50, 50, 25)), robotForwardRange);
				robotSideRange = GUI.TextField(FromBottomRight(new Rect(225, 50, 50, 25)), robotSideRange);
				robotSpeed = GUI.TextField(FromBottomRight(new Rect(150, 50, 50, 25)), robotSpeed);
				// color
				if(robotColorVisible == Color.red) {
					colorInt = 0;
				}
				else if(robotColorVisible == Color.green) {
					colorInt = 1;
				}
				else {
					colorInt = 2;
				}
				colorInt = GUI.Toolbar(FromBottomRight(new Rect(700, 90, 175, 30)),
						       colorInt,
						       new string[] {"Red", "Green", "Blue"});
				switch(colorInt) {
					case 0:
						robotColorVisible = Color.red;
						break;
					case 1:
						robotColorVisible = Color.green;
						break;
					case 2:
						robotColorVisible = Color.blue;
						break;
				}
				// direction
				int dir;
				if(robotFireDirection.y > 0) {
					dir = 0;
				}
				else if(robotFireDirection.x > 0) {
					dir = 1;
				}
				else if(robotFireDirection.y < 0) {
					dir = 2;
				}
				else {
					dir = 3;
				}
				dir = GUI.Toolbar(FromBottomRight(new Rect(500, 90, 175, 30)),
						       dir,
						       new string[] {"North", "East", "South", "West"});
				switch(dir) {
					case 0:
						robotFireDirection = new Vector2(0, 1);
						break;
					case 1:
						robotFireDirection = new Vector2(1, 0);
						break;
					case 2:
						robotFireDirection = new Vector2(0, -1);
						break;
					case 3:
						robotFireDirection = new Vector2(-1, 0);
						break;
				}
				int rotationInt;
				if(robotRotation == new RotationMatrix(RotationMatrix.Rotation.Identity))
					rotationInt = 0;
				if(robotRotation == new RotationMatrix(RotationMatrix.Rotation.Left))
					rotationInt = 1;
				if(robotRotation == new RotationMatrix(RotationMatrix.Rotation.Right))
					rotationInt = 2;
				else
					rotationInt = 3;
				rotationInt = GUI.Toolbar(FromBottomRight(new Rect(300, 100, 50, 10)),
							  rotationInt,
							  new string[] {"Identity", "Left", "Right", "Halfway"});
				switch(rotationInt) {
					case 0:
						robotRotation = new RotationMatrix(RotationMatrix.Rotation.Identity);
						break;
					case 1:
						robotRotation = new RotationMatrix(RotationMatrix.Rotation.Left);
						break;
					case 2:
						robotRotation = new RotationMatrix(RotationMatrix.Rotation.Right);
						break;
					default:
						robotRotation = new RotationMatrix(RotationMatrix.Rotation.Halfway);
						break;
				}
				robotMovementDirection = robotFireDirection;
				break;
			case ObjectType.DestructibleWall:
				// health
				destructibleWallHealth = GUI.TextField(FromBottomRight(new Rect(250, 50, 100, 20)), destructibleWallHealth);
				// color
				if(destructibleWallColor == Color.red) {
					colorInt = 0;
				}
				else if(destructibleWallColor == Color.green) {
					colorInt = 1;
				}
				else if(destructibleWallColor == Color.blue) {
					colorInt = 2;
				}
				else {
					colorInt = 3;
				}
				colorInt = GUI.Toolbar(FromBottomRight(new Rect(300, 90, 250, 30)),
						       colorInt,
						       new string[] {"Red", "Green", "Blue"});
				switch(colorInt) {
					case 0:
						destructibleWallColor = Color.red;
						break;
					case 1:
						destructibleWallColor = Color.green;
						break;
					case 2:
						destructibleWallColor = Color.blue;
						break;
				}
				break;
		}
	}
	
	/*
	 * Sets up the invisible planes for placing objects in the grid.
	 */
	public void SetupObjectPlacers() {
		if(floor == null)
			return;
		// Clear out any old ObjectPlacers
		for(int i = 0; i < objectPlacers.Count; i++) {
			Destroy(objectPlacers[i]);
		}
		objectPlacers.Clear();
		for(int i = 0; i < floor.grid.GetLength(0); i++) {
			for(int j = 0; j < floor.grid.GetLength(1); j++) {
				objectPlacers.Add(ObjectPlacer.MakeObjectPlacer(i, j, floor));
			}
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
