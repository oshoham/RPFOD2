using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class OpeningManager : MonoBehaviour {

	public static bool fadeIn = true;
	public static bool fadeOut = false;
	public float fadeLength;
	public static float fadeStarted;
	public static string nextScene; // Where we'll fade to.
	public Texture fadeTexture;
	public int frame = 0;
	
	void Start () {
		Time.timeScale = 1;
		fadeStarted = Time.time;
		fadeLength = 1;
		fadeTexture = Resources.Load("Textures/single") as Texture;
	}
	
	public static void Load(String scene) {
		nextScene = scene;
		fadeOut = false;
		fadeIn = false;
		fadeStarted = Time.time;
	}

	void Update()	{
		if (frame == 100){
			fadeOut = true;
			fadeStarted = Time.time;
		}
		frame++;
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
			GUI.DrawTexture(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight),
					fadeTexture);
			if(Time.time <= fadeStarted + fadeLength) { // still fading
			}
		}
	}
	
	void OnLevelWasLoaded(int level) {
		fadeStarted = Time.time;
		fadeIn = true;
	}

}