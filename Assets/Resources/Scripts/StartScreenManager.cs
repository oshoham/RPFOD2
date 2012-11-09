using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//using UnityEditor;

//This class manages the start screen.

public class StartScreenManager : MonoBehaviour {

       public static GameObject level1;
       public static GameObject level2;
       public static GameObject editor;

	void Start () {
	     	level1 = GameObject.Find("Level 1");
		level2 = GameObject.Find("Level 2");
		editor = GameObject.Find("Level Editor");
		level2.AddComponent("Level2y");
		level1.AddComponent("MainGame");
		editor.AddComponent("StartScreenEditorButton");
	}
	
	void Update () {
	     
	}
}