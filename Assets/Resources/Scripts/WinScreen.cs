using UnityEngine;

public class WinScreen : MonoBehaviour {
	
	void Start() {
		GameObject winButton = new GameObject("Win Button");
		GUIText win = (GUIText)winButton.AddComponent(typeof(GUIText));
		win.text = "A winner is you!";
		win.anchor = TextAnchor.UpperLeft;
		win.alignment = TextAlignment.Left;
		win.lineSpacing = 1.0F;
		win.font = (Font)Resources.Load("Fonts/ALIEN5");
		win.fontSize = 40;
		winButton.transform.position = new Vector3(0.2F, 0.75F, 0.0F);
		winButton.AddComponent<BackButton>().resizeTo = 50;
	}

	void Update() {

	}
}
