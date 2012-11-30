using UnityEngine;
using System.Collections;

public class StartScreenEditorButton : MonoBehaviour {
       GameObject editor;
       public static int defaultFontSize;
       public static int resizeTo = 50;
       public AudioClip tickOnSelect = Resources.Load("Audio/Effects/click") as AudioClip;	
       public AudioSource effects = new AudioSource();

       void Start() {
       	    editor = GameObject.Find("Editor");
       	    defaultFontSize = editor.guiText.fontSize;
	    effects = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
	    
       }

       void OnMouseEnter() {
       	    editor.guiText.fontSize = resizeTo;
	    effects.clip = tickOnSelect;
	    effects.Play();
       }

       void OnMouseExit() {
       	    editor.guiText.fontSize = defaultFontSize;
       }

       void OnMouseDown() {
       	    Application.LoadLevel("Editor");
       }
}      