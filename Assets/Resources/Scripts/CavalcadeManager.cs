using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

//This class manages the "Cavalcade" play level select screen.

public class CavalcadeManager : MonoBehaviour {

	void Start() {
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

	void Update() {

	}
}
