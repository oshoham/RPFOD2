using UnityEngine;

public class WinScreen : MonoBehaviour {
	GameObject game;
	public static int defaultFontSize;
	public static int resizeTo = 50;
	
	void Start() {
		game = GameObject.Find("Winner Text");
		defaultFontSize = game.guiText.fontSize;
	}
	
	void OnMouseEnter() {
		game.guiText.fontSize = resizeTo;
	}
	
	void OnMouseExit() {
		game.guiText.fontSize = defaultFontSize;
	}
	
	void OnMouseDown() {
		Application.LoadLevel("StartScreen");
	}
}