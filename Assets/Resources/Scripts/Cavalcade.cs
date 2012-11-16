using UnityEngine;
using System.Collections;

public class Cavalcade : MonoBehaviour {
       GameObject game;
       public static int defaultFontSize;
       public static int resizeTo = 50;
       
       void Start() {
       	    game = GameObject.Find("Cavalcade");
       	    defaultFontSize = game.guiText.fontSize;
       }

       void OnMouseEnter() {
       	    game.guiText.fontSize = resizeTo;
       }

       void OnMouseExit() {
       	    game.guiText.fontSize = defaultFontSize;
       }

       void OnMouseDown() {
       	    Application.LoadLevel("Cavalcade");
       }
}      
