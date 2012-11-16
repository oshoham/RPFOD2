using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour {
       public string fileName;
       public static int defaultFontSize;
       public static int resizeTo = 40;
       
       void Start() {
       	    defaultFontSize = this.gameObject.guiText.fontSize;
       }

       void OnMouseEnter() {
       	    this.gameObject.guiText.fontSize = resizeTo;
       }

       void OnMouseExit() {
       	    this.gameObject.guiText.fontSize = defaultFontSize;
       }

       void OnMouseDown() {
	    GlobalSettings.currentFile = fileName;
	    GlobalSettings.lastScene = "FreePlaySelector";
       	    Application.LoadLevel("Game");
       }
}      
