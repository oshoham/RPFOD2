using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

//This class manages the free play level select screen.

public class FreePlayManager : MonoBehaviour {

	public string[] levels;
	public string path;

	void Start () {
		path = "";
		if(Application.isEditor)
			path = Application.dataPath + "/Resources/Levels";
		else
			path = Application.dataPath;
	        levels = Directory.GetFiles(path, "*.txt");
		for(int i = 0; i < levels.Length; i++) {
			string fileName = Path.GetFileName(levels[i]);
			CreateLevelButton(fileName, i);
		}
	}
	
	void Update () {
	     
	}

	void CreateLevelButton(string fileName, int i) {
		string name = Path.GetFileNameWithoutExtension(fileName);
		GameObject button = new GameObject(name + " Button");
		GUIText levelSelector = (GUIText)button.AddComponent(typeof(GUIText));
		levelSelector.text = name;
		levelSelector.anchor = TextAnchor.UpperLeft;
		levelSelector.alignment = TextAlignment.Left;
		levelSelector.lineSpacing = 1.0F;
		levelSelector.font = (Font)Resources.Load("Fonts/ALIEN5");
		levelSelector.fontSize = 40;
		levelSelector.fontStyle = FontStyle.Normal;
		levelSelector.pixelOffset = new Vector2(0, 0);
		button.transform.position = new Vector3(0.2F, (0.75F - i*0.1F), 0.0F);
		LevelButton script = button.AddComponent<LevelButton>();
		script.fileName = fileName;
	}
}
