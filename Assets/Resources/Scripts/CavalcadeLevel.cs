using UnityEngine;
using System.Collections;

public class CavalcadeLevel {
	public Vector3 cameraPos;
	public string filename;

	public CavalcadeLevel(Vector3 cameraPos, string filename) {
		this.cameraPos = cameraPos;
		this.filename = filename;
	}

	public void Load() {
		GlobalSettings.currentFile = filename;
	    	GlobalSettings.lastScene = "CavalcadeMap";
       	    	Application.LoadLevel("Game");
        }
}
