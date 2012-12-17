using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

// This class manages the "Cavalcade" play level select screen.

public class CavalcadeManager : MonoBehaviour {
	
	public static bool fadeIn = true;
	public static bool fadeOut = false;
	public float fadeLength;
	public static float fadeStarted;
	public static string nextScene; // Where we'll fade to.
	public Texture fadeTexture;
	public Texture cavalcadeMap;
	public int levelsCompleted;
	public GameObject map;
	
	void Start() {
		// Button that takes you back to the Start Screen
		GameObject backButton = new GameObject("Back Button");
		GUIText back = (GUIText)backButton.AddComponent(typeof(GUIText));
		back.text = "Back";
		back.anchor = TextAnchor.UpperLeft;
		back.alignment = TextAlignment.Left;
		back.lineSpacing = 1.0F;
		back.font = (Font)Resources.Load("Fonts/ALIEN5");
		back.fontSize = 25;
		backButton.transform.position = new Vector3(0.005F, 0.985F, 0.0F);
		backButton.AddComponent<BackButton>();

		// Determine how many levels the player has completed
		cavalcadeMap = Resources.Load("Textures/map/TitleScreen") as Texture;
		GlobalSettings.lastScene = "CavalcadeManager";
		string path = Path.Combine(Application.persistentDataPath, "SaveFile.txt");
		if(File.Exists(path)) {
			StreamReader reader = new StreamReader(path);
			levelsCompleted = Int32.Parse(reader.ReadLine());
			reader.Close();
			Debug.Log("Levels Completed: " + levelsCompleted);
		}
		else {
			using(StreamWriter writer = File.CreateText(path)) {
				writer.WriteLine(0);
				writer.Close();
				Debug.Log("New save file created at " + path + ", homie.");
			}
			levelsCompleted = 0;
		}
		
		//Draw the actual map on a plane and lighting
		map = GameObject.CreatePrimitive(PrimitiveType.Plane);
		map.name = "Map";
		map.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
		map.renderer.material.mainTexture = Resources.Load("Textures/map/TitleScreen2") as Texture;
		map.renderer.material.shader = Shader.Find("Decal");
		map.transform.Rotate(90, 180, 0);
		map.transform.position = new Vector3(3.5F, 1.25F, 0F);
		GameObject light = new GameObject("Light");
		Light l = light.AddComponent<Light>();
		l.transform.position = new Vector3(3.5F, 1.25F, -5F);
		l.type = LightType.Directional;
		l.intensity = 0.3F;
		Camera.main.transform.position = new Vector3(3.0F, 7F, -18F);
		Camera.main.fieldOfView = 45;
		fadeLength = 5F;
	}
	
	void Update(){
		map.renderer.material.color = Color.Lerp(new Color(0, 0, 0, fadeIn ? 1 : 0),
							 new Color(1, 1, 1, fadeIn ? 0 : 1),
							 (Time.time - fadeStarted)/fadeLength);
		if (Camera.main.transform.position.y > 1.8F) 
			Camera.main.transform.position = new Vector3(3.0F, Camera.main.transform.position.y - 1.5F*Time.deltaTime, -18F);
		if (Camera.main.transform.position.y > 1.5F)
			Camera.main.transform.position = new Vector3(3.0F, Camera.main.transform.position.y - Time.deltaTime, -18F);
		

		if(Input.GetKeyDown("left")) {
		}

		if(Input.GetKeyDown("right")) {
		}
	}
	
	void OnLevelWasLoaded(int level) {
		fadeStarted = Time.time;
		fadeIn = true;
	}
	
	void OnGUI() {
		if(fadeIn || fadeOut) {
			if(Time.time <= fadeStarted + fadeLength) { // still fading
				GUI.color = Color.Lerp(new Color(0, 0, 0, fadeIn ? 1 : 0),
						       new Color(0, 0, 0, fadeIn ? 0 : 1),
						       (Time.time - fadeStarted)/fadeLength);
			}
			else if(fadeIn) { // done fading in
				fadeIn = false;
			}
			else if(fadeOut) { // done fading out
				fadeOut = false;
				fadeIn = true;
				Application.LoadLevel("StartScreen");
			}
//			GUI.DrawTexture(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight),
//					fadeTexture);
		}
//		GUI.DrawTexture(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight), cavalcadeMap, ScaleMode.ScaleToFit);
	}
}
