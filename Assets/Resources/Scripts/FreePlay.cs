using UnityEngine;
using System.Collections;
using System.Threading;

public class FreePlay : MonoBehaviour {
	GameObject game;
	public static int defaultFontSize;
	public static int resizeTo = 50;
	public AudioClip tickOnClick = Resources.Load("Audio/Effects/robotshot") as AudioClip;	
	public AudioClip tickOnHover = Resources.Load("Audio/Effects/tick") as AudioClip;
	public AudioSource effects = new AudioSource();

	void Start() {
		game = GameObject.Find("Free Play");
		defaultFontSize = game.guiText.fontSize;
		effects = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
	}
	
	void OnMouseEnter() {
		game.guiText.fontSize = resizeTo;
		effects.clip = tickOnClick;
		effects.Play();
	}
	
	void OnMouseExit() {
		game.guiText.fontSize = defaultFontSize;
	}
	
	void OnMouseDown() {
		effects.clip = tickOnClick;
		effects.Play();
		Application.LoadLevel("FreePlaySelector");
	}	

}