using UnityEngine;

public class WinScreen : MonoBehaviour {

	public AudioSource bgm = new AudioSource();
	public AudioClip song;
	
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
		BackButton b = winButton.AddComponent<BackButton>();
		b.resizeTo = 50;
		b.destination = "FreePlaySelector";
		bgm = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
		song = Resources.Load("Audio/Effects/winsong") as AudioClip;
		bgm.loop = true;
		bgm.clip = song;
		bgm.Play();
	}

	void Update() {
		if (Input.GetKeyDown("space"))
			Application.LoadLevel("FreePlaySelector");
	}
}
