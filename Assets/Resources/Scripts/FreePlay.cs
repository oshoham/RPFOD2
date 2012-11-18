using UnityEngine;
using System.Collections;

public class FreePlay : MonoBehaviour {
       GameObject game;
       public static int defaultFontSize;
       public static int resizeTo = 50;
       public AudioClip tickOnSelect = Resources.Load("Audio/Effects/robotshot") as AudioClip;	
       public AudioSource effects = new AudioSource();

       void Start() {
       	    game = GameObject.Find("Free Play");
       	    defaultFontSize = game.guiText.fontSize;
	    effects = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
       }

       void OnMouseEnter() {
       	    game.guiText.fontSize = resizeTo;
	    effects.clip = tickOnSelect;
	    effects.Play();
       }

       void OnMouseExit() {
       	    game.guiText.fontSize = defaultFontSize;
       }

       void OnMouseDown() {
       	    Application.LoadLevel("FreePlaySelector");
       }
}