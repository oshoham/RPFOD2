using UnityEngine;
using System.Collections;

public class StartScreenEditorButton : MonoBehaviour {
       GameObject editor;
       public static int defaultFontSize;
       public static int resizeTo = 50;

       void Start() {
       	    editor = GameObject.Find("Editor");
       	    defaultFontSize = editor.guiText.fontSize;
       }

       void OnMouseEnter() {
       	    editor.guiText.fontSize = resizeTo;
       }

       void OnMouseExit() {
       	    editor.guiText.fontSize = defaultFontSize;
       }

       void OnMouseDown() {
       	    Application.LoadLevel("Editor");
       }
}      