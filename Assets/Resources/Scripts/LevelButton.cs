using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour {
       public string fileName;
       public static int defaultFontSize;
       public static int resizeTo = 40;
	public AudioSource effects = new AudioSource();
	public AudioClip click;
       
       void Start() {
       	    defaultFontSize = this.gameObject.guiText.fontSize;
	    click = Resources.Load("Audio/Effects/click") as AudioClip;
	    effects = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
       }

       void OnMouseEnter() {
       	    this.gameObject.guiText.fontSize = resizeTo;
	    effects.clip = click;
	    effects.Play();
       }

       void OnMouseExit() {
       	    this.gameObject.guiText.fontSize = defaultFontSize;
       }

       void OnMouseDown() {
	    GlobalSettings.currentFile = fileName;
	    GlobalSettings.lastScene = "FreePlaySelector";
       	    FreePlayManager.LoadGame();
       }
}      
