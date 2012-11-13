using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour {
       public string fileName;
       public static int defaultFontSize;
       public static int resizeTo = 50;
       
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
       	    Application.LoadLevel("Game");
       }
}      
