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
		int j = 1;
		for(int i = 0; i < levels.Length; i++) {
			string fileName = Path.GetFileName(levels[i]);
			if(i>=10) j=2;
			if(i>=20) j=3;
			if(i>=30) j=4;
			if(i>=40) j=5;
			if(i>=50) j=6;
			CreateLevelButton(fileName, i, j);
		}
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
		switch(j)
		{
			case 1:
				button.transform.position = new Vector3(0.05F, (0.95F - i*0.1F), 0.0F);
				break;
			case 2:
				button.transform.position = new Vector3(0.2F, (0.95F - (i-10)*0.1F), 0.0F);
				break;
			case 3:
				button.transform.position = new Vector3(0.35F, (0.95F - (i-20)*0.1F), 0.0F);
				break;
			case 4:
				button.transform.position = new Vector3(0.5F, (0.95F - (i-30)*0.1F), 0.0F);
				break;
			case 5:
				button.transform.position = new Vector3(0.65F, (0.95F - (i-40)*0.1F), 0.0F);
				break;
			case 6:
				button.transform.position = new Vector3(0.8F, (0.95F - (i-50)*0.1F), 0.0F);
				break;
		}	     
		LevelButton script = button.AddComponent<LevelButton>();
		script.fileName = fileName;
	}
}
