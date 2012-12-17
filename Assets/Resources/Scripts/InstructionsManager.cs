using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class InstructionsManager : MonoBehaviour {

	public static bool fadeIn = true;
	public static bool fadeOut = false;
	public float fadeLength;
	public static float fadeStarted;
	public static string nextScene; // Where we'll fade to.
	public Texture fadeTexture;

	
	void Start () {
		Time.timeScale = 1;
		fadeStarted = Time.time;
		fadeLength = 1F;
		fadeTexture = Resources.Load("Textures/single") as Texture;

		// Button that takes you back to the Start Screen
		GameObject backButton = new GameObject("Back Button");
		GUIText back = (GUIText)backButton.AddComponent(typeof(GUIText));
		back.text = "Back";
		back.anchor = TextAnchor.UpperLeft;
		back.alignment = TextAlignment.Left;
		back.lineSpacing = 1.0F;
		back.font = (Font)Resources.Load("Fonts/ALIEN5");
		back.fontSize = 25;
		back.material.color = Color.white;
		backButton.transform.position = new Vector3(0.005F, 0.985F, 0.0F);
		backButton.AddComponent<BackButton>();

		GameObject instructionsText = new GameObject("Instructions Set");
		GUIText instruct = (GUIText)instructionsText.AddComponent(typeof(GUIText));
		instruct.text = "Keybindings:\n\nW: move up\nS: move down\nA: move left\nD: move right\n(or Arrow keys)\n\nQ/E/U/O: cycle what color you're shooting\n\nJ/1: turn red\nK/2: turn green\nL/3: turn blue\nI/4: turn default color\n\nSpace: shoot paint\n\nR: restart level\n\n\nGoal:\nuse paint to escape the robots or destroy them all";
		instruct.anchor = TextAnchor.UpperLeft;
		instruct.alignment = TextAlignment.Left;
		instruct.lineSpacing = 1.0F;
		instruct.font = (Font)Resources.Load("Fonts/ALIEN5");
		instruct.fontSize = 30;
		instruct.material.color = Color.white;
		instruct.transform.position = new Vector3(0.1F, 0.97F, 0.0F);

	}
	
	public static void Load(String scene) {
		nextScene = scene;
		fadeOut = false;
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