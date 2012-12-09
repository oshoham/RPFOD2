using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

//This class manages the free play level select screen.

public class FreePlayManager : MonoBehaviour {

	public string[] levels;
	public string path;
	public AudioClip fplaysong;
	public AudioClip tickOnHover;
	public AudioSource soundtrack = new AudioSource();
	public AudioSource effects = new AudioSource();
	
	public static bool fadeIn = true;
	public static bool fadeOut = false;
	public float fadeLength;
	public static float fadeStarted;
	public Texture fadeTexture;
	
	void Start () {
		Time.timeScale = 1;
		fadeStarted = Time.time;
		fadeLength = 1;
		fadeTexture = Resources.Load("Textures/single") as Texture;
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

		path = "";
		if(Application.isEditor)
			path = Application.dataPath + "/Resources/Levels";
		else
			path = Application.dataPath;
	        levels = Directory.GetFiles(path, "*.txt");
		int x = 50;
		int y = 50;
		int textHeight = 50;
		int textWidth = 200;
		for(int i = 0; i < levels.Length; i++, y += textHeight) {
			string fileName = Path.GetFileName(levels[i]);
			if(y > Camera.main.pixelHeight - textHeight) {
				y = 50;
				x += textWidth;
			}
			if(x > Camera.main.pixelWidth)
				print("shit, bro! the x is too damn high!");
			CreateLevelButton(fileName, x, (int)Camera.main.pixelHeight - y);
		}
		//audio
		fplaysong = Resources.Load("Audio/JaiDormiSousLeauChateauFlightRemix") as AudioClip;
		tickOnHover = Resources.Load("Audio/Effects/click") as AudioClip;
		effects = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
		soundtrack = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
		soundtrack.loop = true;
		soundtrack.clip = fplaysong;
		soundtrack.volume = 0.1f;
	        soundtrack.Play();
	}
	
	void Update () {
	     
	}

	void CreateLevelButton(string fileName, int i, int j) {
		string name = Path.GetFileNameWithoutExtension(fileName);
		GameObject button = new GameObject(name + " Button");
		GUIText levelSelector = (GUIText)button.AddComponent(typeof(GUIText));
		levelSelector.text = name;
		levelSelector.anchor = TextAnchor.UpperLeft;
		levelSelector.alignment = TextAlignment.Left;
		levelSelector.lineSpacing = 0.5F;
		levelSelector.font = (Font)Resources.Load("Fonts/ALIEN5");
		levelSelector.fontSize = 30;
		levelSelector.fontStyle = FontStyle.Normal;
		levelSelector.pixelOffset = new Vector2(0, 0);
		button.transform.position = Camera.main.ScreenToViewportPoint(new Vector3(i, j, 0));
		LevelButton script = button.AddComponent<LevelButton>();
		script.fileName = fileName;
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
				Application.LoadLevel("Game");
			}
			GUI.DrawTexture(new Rect(0, 0, Camera.main.pixelWidth, Camera.main.pixelHeight),
					fadeTexture);
		}
	}
	
	void OnLevelWasLoaded(int level) {
		fadeStarted = Time.time;
		fadeIn = true;
	}

	public static void LoadGame() {
		fadeOut = true;
		fadeIn = false;
		fadeStarted = Time.time;
	}
}
