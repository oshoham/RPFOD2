using UnityEngine;
using System.Collections;

public class CavalcadeButton: MonoBehaviour {
	public string filename;
	public Vector3 defaultSize;
	public Vector3 resizeTo;
	public AudioSource effects = new AudioSource();
	public AudioClip click;

	void Start() {
		click = Resources.Load("Audio/Effects/click") as AudioClip;
		effects = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
		defaultSize = transform.localScale;
	}

	void OnMouseEnter() {
		effects.clip = click;
	 	effects.Play();
		transform.localScale = resizeTo;
        }

	void OnMouseExit() {
		transform.localScale = defaultSize;
	}

	void OnMouseDown() {
		GlobalSettings.currentFile = filename;
	    	GlobalSettings.lastScene = "CavalcadeMap";
       	    	Application.LoadLevel("Game");
        }
}
