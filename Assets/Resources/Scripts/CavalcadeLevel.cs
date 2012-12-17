using UnityEngine;
using System.Collections;

public class CavalcadeLevel {
	public Vector2 cameraPos;
	public string filename;

	void Load() {
		GlobalSettings.currentFile = filename;
	    	GlobalSettings.lastScene = "CavalcadeMap";
       	    	Application.LoadLevel("Game");
        }
}
