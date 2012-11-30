using UnityEngine;
using System.Collections;

public class Cavalcade : MonoBehaviour {
	GameObject game;
	public static int defaultFontSize;
	public static int resizeTo = 50;
	public AudioClip tickOnSelect = Resources.Load("Audio/Effects/click") as AudioClip;	
	public AudioSource effects = new AudioSource();
       
       void Start() {
       	    game = GameObject.Find("Cavalcade");
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
	       StartScreenManager.Load("CavalcadeMap");
       }
}      
