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
	public CavalcadeLevel[] levels;
	public int levelIndex;
	public GameObject indicator;
	public bool scrolled = false;
	public int frame = 0;
	public bool scaleDown = false;
	public AudioSource bgm = new AudioSource();
	public AudioClip song;
	
	void Start() {
		levels = new CavalcadeLevel[10];
		levels[0] = new CavalcadeLevel(new Vector3(-1.8F,-1.45F,-18F), "L5.txt");
		levels[1] = new CavalcadeLevel(new Vector3(0,1F,-18F), "L6.txt");
		levels[2] = new CavalcadeLevel(new Vector3(0F,5.3F,-18F), "KeepAlive.txt");
		levels[3] = new CavalcadeLevel(new Vector3(2.3F,5.3F,-18F), "KeepAlive2.txt");
		levels[4] = new CavalcadeLevel(new Vector3(1.18F,0.77F,-18F), "Covertop.txt");
		levels[5] = new CavalcadeLevel(new Vector3(1.18F,-2.17F,-18F), "Whirlpool.txt");
		levels[6] = new CavalcadeLevel(new Vector3(5.57F,-2.17F,-18F), "KillAllHumans.txt");
		levels[7] = new CavalcadeLevel(new Vector3(5.57F,0F,-18F), "Virus.txt");
		levels[8] = new CavalcadeLevel(new Vector3(7.33F,4.7F,-18F), "RGB.txt");
		levels[9] = new CavalcadeLevel(new Vector3(7.33F,9.3F,-18F), "Olympus.txt");

		bgm = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
		song = Resources.Load("Audio/Enter The Machine") as AudioClip;
		bgm.loop = true;
		bgm.clip = song;
		bgm.Play();
		transform.position = Camera.main.transform.position;
		transform.parent = Camera.main.transform;

		// Button that takes you back to the Start Screen
		GameObject backButton = new GameObject("Back Button");
		GUIText back = (GUIText)backButton.AddComponent(typeof(GUIText));
		back.text = "Back";
		back.anchor = TextAnchor.UpperLeft;
		back.alignment = TextAlignment.Left;
		back.lineSpacing = 1.0F;
		back.font = (Font)Resources.Load("Fonts/ALIEN5");
		back.fontSize = 35;
		back.material.color = Color.black;
		backButton.transform.position = new Vector3(0.005F, 0.985F, 0.0F);
		backButton.AddComponent<BackButton>();

		GameObject controlInfo = new GameObject("Control Information");
		GUIText text = (GUIText)controlInfo.AddComponent(typeof(GUIText));
		text.text = "Left and\nRight to\nSwitch\nLevels\n\nEnter or\nSpace to\nChoose a\nLevel";
		text.font = (Font)Resources.Load("Fonts/ALIEN5");
		text.material.color = Color.black;
		text.fontSize = 25;
		controlInfo.transform.position = new Vector3(0.005F, 0.925F, 0.0F);

		// Determine how many levels the player has completed
		cavalcadeMap = Resources.Load("Textures/map/TitleScreen") as Texture;
		GlobalSettings.lastScene = "CavalcadeManager";
		string path = Path.Combine(Application.dataPath, "SaveFile.txt");
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

		levelIndex = levelsCompleted;
		if(levelIndex == 10)
			levelIndex = 9;
		
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
		Camera.main.transform.position = new Vector3(-1.8F, -1.45F, -18F);
		Camera.main.fieldOfView = 45;
		fadeLength = 5F;

		//Draw the first indicator
		indicator = GameObject.CreatePrimitive(PrimitiveType.Plane);
		indicator.name = "Indicator";
		indicator.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		indicator.transform.Rotate(90,180,0);
		indicator.renderer.material.mainTexture = Resources.Load("Textures/PlayerReal") as Texture;
		indicator.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		indicator.renderer.material.SetColor("_Color",new Vector4(1F,1F,1F,1F));
		indicateLevel();
	}
	
	void Update(){
		map.renderer.material.color = Color.Lerp(new Color(0, 0, 0, fadeIn ? 1 : 0),
							 new Color(1, 1, 1, fadeIn ? 0 : 1),
							 (Time.time - fadeStarted)/fadeLength);

		if(!scrolled){
			if (Camera.main.transform.position.y > 1.8F) 
				Camera.main.transform.position = new Vector3(3.0F, Camera.main.transform.position.y - 1.5F*Time.deltaTime, -18F);
			if (Camera.main.transform.position.y > 1.5F)
				Camera.main.transform.position = new Vector3(3.0F, Camera.main.transform.position.y - Time.deltaTime, -18F);
			scrolled = true;
		}

		if(Input.GetKeyDown("left")) {
			if(levelIndex > 0) {
				levelIndex--;
				Camera.main.transform.position = levels[levelIndex].cameraPos;
				indicateLevel();
			}
		}

		if(Input.GetKeyDown("right")) {
			if(levelIndex < 9) {
				levelIndex++;
				Camera.main.transform.position = levels[levelIndex].cameraPos;
				indicateLevel();
			}
		}

		if(Input.GetKeyDown("space") || Input.GetKeyDown("enter"))
			levels[levelIndex].Load();

		if(indicator.transform.localScale == new Vector3(0.25F, 0.25F, 0.25F))
			scaleDown = true;
		
		if(indicator.transform.localScale == new Vector3(0.2F, 0.2F, 0.2F))
			scaleDown = false;

		if(scaleDown == false)
			indicator.transform.localScale += new Vector3(0.001F, 0.001F, 0.001F);
		if(scaleDown == true)
			indicator.transform.localScale -= new Vector3(0.001F, 0.001F, 0.001F);
		   

	}

	void indicateLevel()
	{
		if(levelIndex==0)
			indicator.transform.position = new Vector3(-5.0F, -2.2F, -1F);
		if(levelIndex==1)
			indicator.transform.position = new Vector3(-3.75F, 1.57F, -1F);
		if(levelIndex==2)
			indicator.transform.position = new Vector3(-3.0F, 4.4F, -1F);
		if(levelIndex==3)
			indicator.transform.position = new Vector3(2.1F, 5.5F, -1F);
		if(levelIndex==4)
			indicator.transform.position = new Vector3(1.64F, -0.72F, -1F);
		if(levelIndex==5)
			indicator.transform.position = new Vector3(2.0F, -4.5F, -1F);
		if(levelIndex==6)
			indicator.transform.position = new Vector3(8.55F, -2.5F, -1F);
		if(levelIndex==7)
			indicator.transform.position = new Vector3(7.0F, 0.2F, -1F);
		if(levelIndex==8)
			indicator.transform.position = new Vector3(7.7F, 3.2F, -1F);
		if(levelIndex==9)
			indicator.transform.position = new Vector3(10.1F, 9.4F, -1F);
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
