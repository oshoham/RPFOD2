using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

//This class manages the "Cavalcade" play level select screen.

public class CavalcadeManager : MonoBehaviour {
	
	public static bool fadeIn = true;
	public static bool fadeOut = false;
	public float fadeLength;
	public static float fadeStarted;
	public static string nextScene; // Where we'll fade to.
	public Texture fadeTexture;
	public Texture cavalcadeMap;
	
	void Start() {
		Time.timeScale = 1;
		fadeStarted = Time.time;
		fadeLength = 1;
		fadeTexture = Resources.Load("Textures/single") as Texture;
		cavalcadeMap = Resources.Load("Textures/cavalcademap") as Texture;
		GameObject winButton = new GameObject("Placeholder");
		GUIText win = (GUIText)winButton.AddComponent(typeof(GUIText));
		win.text = "Nothing to see here...yet.";
		win.anchor = TextAnchor.UpperLeft;
		win.alignment = TextAlignment.Left;
		win.lineSpacing = 1.0F;
		win.font = (Font)Resources.Load("Fonts/ALIEN5");
		win.fontSize = 40;
		winButton.transform.position = new Vector3(0.2F, 0.75F, 0.0F);
		winButton.AddComponent<BackButton>().resizeTo = 50;
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
			GUI.DrawTexture(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight),
					fadeTexture);
		}
	}
}
