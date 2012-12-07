using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour, IColor {

	public Grid grid;
	
	public int health;
	public Vector2 gridCoords;

	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			_colorPainted = value;
			gameObject.renderer.material.color = value;
		}
	}
	public Color colorShooting;
	public Dictionary<Color, int> colors;

	public float moveSpeed;
	public float startedMoving;
	public float endMoving;
	public Vector3 oldPosition;
	public Vector3 newPosition;

	public float lastMovedHorizontal;
	public float lastMovedVertical;
	public float moveRate;
	
	public Color defaultColor;
	public Vector2 dir = new Vector2(1, 0);
	public GameObject explosion = Resources.Load("Standard Assets/Particles/Legacy Particles/explosion") as GameObject;

	public bool dead;
	public List<string> awfulQuotes;	

	//audio
	public AudioSource peffects = new AudioSource();
	public AudioClip soundexplosion;
	public AudioClip paintshot;
        public bool explohappened = false;

	void Start() {
		awfulQuotes.Add("Death comes swiftest to those who die. -- JFK");
		awfulQuotes.Add("We've got the best forensics tool money can't buy. Snow. -- CSI: NY");
		awfulQuotes.Add("Mother died today. Or maybe yesterday; I can't be sure. -- Albert Camus");
		awfulQuotes.Add("If the human brain were so simple that we could understand it, we would be so simple that we couldn't. -- Emerson M. Pugh");
		awfulQuotes.Add("I've resolved to renounce embarrassment in favor of enjoyment. -- Thomas Jefferson");
		awfulQuotes.Add("To doubt everything or to believe everything are two equally convenient solutions; both dispense with the necessity of reflection. -- Henri Poincaré");
		awfulQuotes.Add("For a 6-foot-3 guy with no hair and a whiny voice, I've done all right. -- Billy Corgan");
		soundexplosion = Resources.Load("Audio/Effects/explosion") as AudioClip;
		paintshot = Resources.Load("Audio/Effects/paintshot") as AudioClip;
		peffects = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
	}
	
	/*
	 * Haha! Algorithms!
	 */
	// public static string BreakLines(string text, int charsPerLine) {
	// 	string[] parts = text.Split(new char[] {'-'}); // Split into quote and source
	// 	string[] quote = parts[0].Split(new char[] {' '}); // Split quote into words
	// 	//		StringBuilder sb = new StringBuilder();
	// }
	
	void Update() {
		if(health <= 0) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			if(!explohappened)
			{
				peffects.clip = soundexplosion;
				peffects.volume = 1.0f;
				peffects.Play();
				explohappened = true;
			}
			if(!dead) {
				GameObject JFK = new GameObject("JFK");
				GUIText back = (GUIText)JFK.AddComponent(typeof(GUIText));
				System.Random rand = new System.Random();
				int quoteIndex = rand.Next(awfulQuotes.Count);
				string quote = awfulQuotes[quoteIndex];
				string[] split = quote.Split(new Char[] {' '});
				if(split.Length >= 7) {
					split[6] = split[6] + "\n";
					if(split.Length >= 14)
						split[13] = split[13] + "\n";
					quote = String.Join(" ", split);
				}
				back.text = quote;
				back.anchor = TextAnchor.UpperLeft;
				back.alignment = TextAlignment.Left;
				back.lineSpacing = 1.0F;
				back.font = (Font)Resources.Load("Fonts/ALIEN5");
				back.fontSize = 40;
				JFK.transform.position = new Vector3(0.25F, 0.3F, 0.5F);
				JFK.AddComponent<BackButton>().resizeTo = 45;
				dead = true;
			}
			return;
		}
		GetKeypresses();
		// Make sure the camera follows the Player if we're not in the editor
		if(Time.timeScale > 0) {
			Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,
								     Camera.main.transform.position.z);
		}
	}
	
	public void GetKeypresses() {
		if(Time.time > endMoving) {
			if(Input.GetKey("w")) {
				if(Time.time - lastMovedVertical > moveRate) {
					dir = new Vector2(0, 1);
					Move(dir);
					lastMovedVertical = Time.time;
					transform.localEulerAngles = new Vector3(0, 0, 270f);
				}
			}
			if(Input.GetKey("a")) {
				if(Time.time - lastMovedHorizontal > moveRate) {
					dir = new Vector2(-1, 0);
					Move(dir);
					lastMovedHorizontal = Time.time;
					transform.localEulerAngles = new Vector3(0, 0, 360f);
				}
			}
			if(Input.GetKey("s")) {
				if(Time.time - lastMovedVertical > moveRate) {
					dir = new Vector2(0, -1);
					Move(dir);
					lastMovedVertical = Time.time;
					transform.localEulerAngles = new Vector3(0, 0, 90f);
				}
			}
			if(Input.GetKey("d")) {
				if(Time.time - lastMovedHorizontal > moveRate) {
					dir = new Vector2(1, 0);
					Move(dir);
					lastMovedHorizontal = Time.time;
					transform.localEulerAngles = new Vector3(0, 0, 180f);
				}
			}
		}
		if(Input.GetKeyDown("1")) {
			colorPainted = defaultColor;
		}
		if(Input.GetKeyDown("2") && colors.ContainsKey(Color.red) && colors[Color.red] > 0) {
			ReassignColor(Color.red);
		}
		if(Input.GetKeyDown("3") && colors.ContainsKey(Color.green) && colors[Color.green] > 0) {
			ReassignColor(Color.green);
		}
		if(Input.GetKeyDown("4") && colors.ContainsKey(Color.blue) && colors[Color.blue] > 0) {
			ReassignColor(Color.blue);
		}
		if(Input.GetKeyDown("space") && colors.ContainsKey(colorShooting) && colors[colorShooting] > 0) {
			Paintball.MakePaintball(transform.position, dir, colorShooting, gameObject);
			//make audio
		        peffects.clip = paintshot;
			peffects.Play();
			colors[colorShooting]--;
			if(colors[colorShooting] == 0) {
				colorShooting = colors.FirstOrDefault((KeyValuePair<Color, int> kvp) => kvp.Value > 0).Key;
			}
		}
		AnimateMotion();
	}
	
	/*
	 * If we've just run out of one color, reset our colorShooting to
	 * something we do have.
	 */
	public void ReassignColor(Color color) {
		colorPainted = color;
		colors[color]--;
		if(colors[color] == 0) {
			colorShooting = colors.FirstOrDefault((KeyValuePair<Color, int> kvp) => kvp.Value > 0).Key;
		}
	}
	
	/*
	 * Move by x and y in the game grid. The coordinates should probably be in the
	 * range (-1, 1).
	 */
	public void Move(Vector2 coords) {
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject)) {
			gridCoords += coords;
			startedMoving = Time.time;
			endMoving = startedMoving + moveSpeed;
			oldPosition = transform.position;
			newPosition = new Vector3(gridCoords.x, gridCoords.y, -0.5f);
		}
	}
	
	/*
	 * For smooth motion animation.
	 */
	public void AnimateMotion() {
		if(Time.time >= endMoving) {
			return;
		}
		float time = (Time.time - startedMoving)/moveSpeed + .1f;
		transform.position = Vector3.Lerp(oldPosition, newPosition, time);
	}
	
	public void PickupColor(Color color) {
		if(colors.ContainsKey(color)) {
			colors[color]++;
		}
		else {
			colors.Add(color, 1);
		}
		if(colorShooting == default(Color)) {
			colorShooting = color;
		}
	}
	
	void OnDisable() {
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
	
	public static GameObject MakePlayer(Grid grid, int x, int y, int health) {
		GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
		player.name = "Player";
		player.renderer.material.mainTexture = Resources.Load("Textures/PlayerReal") as Texture;
		player.renderer.material.color = Color.white;
		player.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		player.transform.position = new Vector3(x, y, -0.5f);
		Player script = player.AddComponent<Player>();
		script.gridCoords = new Vector2(x, y);
		script.health = health;
		script.defaultColor = player.renderer.material.color;
		script.oldPosition = player.transform.position;
		script.newPosition = player.transform.position;
		script.startedMoving = Time.time;
		script.endMoving = Time.time;
		script.lastMovedHorizontal = Time.time;
		script.lastMovedVertical = Time.time;
		script.moveRate = 0.1f;
		script.moveSpeed = 0.2f;
		script.colors = new Dictionary<Color, int>();
		script.colorPainted = script.defaultColor;
		script.collider.enabled = true;
		script.grid = grid;
		script.awfulQuotes = new List<string>();
		return player;
	}
}
