using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour {
       public string fileName;
       public static int defaultFontSize;
       public static int resizeTo = 40;
	public AudioSource effects = new AudioSource();
	public AudioClip tick;
       
       void Start() {
       	    defaultFontSize = this.gameObject.guiText.fontSize;
	    tick = Resources.Load("Audio/Effects/tick") as AudioClip;
	    effects = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
       }

       void OnMouseEnter() {
       	    this.gameObject.guiText.fontSize = resizeTo;
	    effects.clip = tick;
	    effects.Play();
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
