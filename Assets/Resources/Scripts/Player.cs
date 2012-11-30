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

	void Start() {
		awfulQuotes.Add("Death comes swiftest to those who die. -JFK");
	}

	void Update() {
		if(health <= 0) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			if(!dead) {
				GameObject JFK = new GameObject("JFK");
				GUIText back = (GUIText)JFK.AddComponent(typeof(GUIText));
				System.Random rand = new System.Random();
				int quoteIndex = rand.Next(awfulQuotes.Count);
				back.text = awfulQuotes[quoteIndex];
				back.anchor = TextAnchor.UpperLeft;
				back.alignment = TextAlignment.Left;
				back.lineSpacing = 1.0F;
				back.font = (Font)Resources.Load("Fonts/ALIEN5");
				back.fontSize = 40;
				JFK.transform.position = new Vector3(0.25F, 0.3F, 0.5F);
				JFK.AddComponent<BackButton>().resizeTo = 45;
				dead = true;
			}
		}
		GetKeypresses();
		// Make sure the camera follows the Player if we're not in the editor
		if(Time.timeScale > 0) {
			Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,
								     Camera.main.transform.position.z);
		}
//		if(GameManager.plane != null) {
//			GameManager.plane.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(50, Camera.main.pixelHeight - 40, Camera.main.nearClipPlane+6));
//		}
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
		transform.position = CubicInterpolate(oldPosition, newPosition, time);//Vector3.Lerp(oldPosition, newPosition, time);
	}
	
	/*
	 * f(x) = -2x^3 + 3x^2
	 */
	public static Vector3 CubicInterpolate(Vector3 oldPosition, Vector3 newPosition, float time) {
		if(time < 0)
			time = 0;
		if(time > 1)
			time = 1;
		float f = (float)(-2*Math.Pow(time, 3) + 3*Math.Pow(time, 2));
		Vector3 position = new Vector3();
		position.x = (newPosition.x - oldPosition.x)*f + oldPosition.x;
		position.y = (newPosition.y - oldPosition.y)*f + oldPosition.y;
		position.z = (newPosition.z - oldPosition.z)*f + oldPosition.z;
		return position;
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
