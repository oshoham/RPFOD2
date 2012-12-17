using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StartScreenManager : MonoBehaviour {

       public static GameObject cavalcade;
       public static GameObject freeplay;
       public static GameObject editor;
	public static GameObject instructions;
	
	public static bool fadeIn = true;
	public static bool fadeOut = false;
	public float fadeLength;
	public static float fadeStarted;
	public static string nextScene; // Where we'll fade to.
	public Texture fadeTexture;
	
	void Start () {
		Time.timeScale = 1;
		fadeStarted = Time.time;
		fadeLength = 1;
		fadeTexture = Resources.Load("Textures/single") as Texture;
	        cavalcade = GameObject.Find("Cavalcade");
		freeplay = GameObject.Find("Free Play");
		editor = GameObject.Find("Editor");
		instructions = GameObject.Find("Instructions");
		cavalcade.AddComponent("Cavalcade");
		freeplay.AddComponent("FreePlay");
		editor.AddComponent("StartScreenEditorButton");
		instructions.AddComponent("InstructionsButton");
	}
	
	public static void Load(String scene) {
		nextScene = scene;
		fadeOut = true;
		fadeIn = false;
		fadeStarted = Time.time;
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
				Application.LoadLevel(nextScene);
			}
			GUI.DrawTexture(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight),
					fadeTexture);
		}
	}
	
	void OnLevelWasLoaded(int level) {
		fadeStarted = Time.time;
		fadeIn = true;
	}
}