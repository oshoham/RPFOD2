using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//This class manages the free play level select screen.

public class FreePlayManager : MonoBehaviour {

	void Start () {
	        
	}
	
	void Update () {
	     
	}

	void CreateLevelButton(string levelName, string fileName, int i) {
		GameObject button = new GameObject(levelName + " Button");
		GUIText levelSelector = (GUIText)button.AddComponent(typeof(GUIText));
		levelSelector.text = levelName;
		levelSelector.anchor = TextAnchor.UpperLeft;
		levelSelector.alignment = TextAlignment.Left;
		levelSelector.lineSpacing = 1.0F;
		levelSelector.font = (Font)Resources.Load("Fonts/ALIEN5");
		levelSelector.fontSize = 40;
		levelSelector.fontStyle = FontStyle.Normal;
		levelSelector.pixelOffset = new Vector2(0, 0);
		button.transform.position = new Vector3(0.2F, (0.75F - i*0.1F), 0.0F);
		LevelButton script = button.AddComponent<LevelButton>();
		script.levelName = levelName;
		script.fileName = fileName;
	}
}
