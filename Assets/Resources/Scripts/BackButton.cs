using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {
       public static int defaultFontSize;
       public int resizeTo = 30;
       
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
       	    Application.LoadLevel("StartScreen");
       }
}      
