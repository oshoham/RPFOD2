using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//using UnityEditor;

//This class manages the start screen.

public class StartScreenManager : MonoBehaviour {

       public static GameObject cavalcade;
       public static GameObject freeplay;
       public static GameObject editor;

	void Start () {
	        cavalcade = GameObject.Find("Cavalcade");
		freeplay = GameObject.Find("Free Play");
		editor = GameObject.Find("Editor");
		cavalcade.AddComponent("Cavalcade");
//		freeplay.AddComponent("FreePlay");
		editor.AddComponent("StartScreenEditorButton");
	}
	
	void Update () {
	     
	}
}