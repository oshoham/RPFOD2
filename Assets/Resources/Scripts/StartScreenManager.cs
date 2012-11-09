using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;

//This class manages the start screen.

public class StartScreenManager : MonoBehaviour {

       public static GameObject game;
       public static GameObject editor;

	void Start () {
	     	game = GameObject.Find("Main Game");
		editor = GameObject.Find("Level Editor");
		game.AddComponent("MainGame");
		editor.AddComponent("StartScreenEditorButton");
	}
	
	void Update () {
	     
	}
}