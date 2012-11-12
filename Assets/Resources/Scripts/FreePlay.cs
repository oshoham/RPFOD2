using UnityEngine;
using System.Collections;

public class FreePlay : MonoBehaviour {
       GameObject game;
       public static int defaultFontSize;
       public static int resizeTo = 50;
       
       void Start() {
       	    game = GameObject.Find("Free Play");
       	    defaultFontSize = game.guiText.fontSize;
       }

       void OnMouseEnter() {
       	    game.guiText.fontSize = resizeTo;
       }

       void OnMouseExit() {
       	    game.guiText.fontSize = defaultFontSize;
       }

       void OnMouseDown() {
       	    Application.LoadLevel("FreePlaySelector");
       }
}