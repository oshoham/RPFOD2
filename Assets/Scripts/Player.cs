using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public int health;
	public Vector2 gridCoords;
	public Color colorPainted;
	public Color colorShooting;
	public List<Color> colors;
	public int colorIndex;
	
	void Start() {
		colors = new List<Color>(4);
		colors.Add(renderer.material.color);
	}

	void Update() {
		if(health <= 0) {
			Destroy(gameObject);
		}
		GetKeypresses();
	}

	public void GetKeypresses() {
		if(Input.GetKeyDown("w")) {
			Move(new Vector2(0, 1));
		}
		if(Input.GetKeyDown("a")) {
			Move(new Vector2(-1, 0));
		}
		if(Input.GetKeyDown("s")) {
			Move(new Vector2(0, -1));
		}
		if(Input.GetKeyDown("d")) {
			Move(new Vector2(1, 0));
		}
		if(Input.GetKeyDown("q")) {
			CycleColorPainted();
		}
	}

	/*
	 * Increments the index which we're at in the colors list, and sets the
	 * current color painted to that color.
	 */
	public void CycleColorPainted() {
		if(colors.Count > 0) {
			SetColorPainted(colors[++colorIndex % colors.Count]);
		}
	}
	
	/*
	 * Move by x and y in the game grid. The coordinates should probably be in the
	 * range (-1, 1).
	 */
	public void Move(Vector2 coords) {
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject)) {
			gridCoords += coords;
			transform.Translate(new Vector3(coords.x, coords.y, 0));
			Camera.main.transform.Translate(new Vector3(coords.x, coords.y, 0));
		}
	}

	/*
	 * Checks if we've already got the given color, and adds it in the order
	 * red -> green -> blue if we don't.
	 */
	public void PickupColor(Color color) {
		if(colors.Contains(color)) {
			return;
		}
		if(color == Color.blue) {
			colors.Add(color);
		}
		else if(color == Color.red) {
			colors.Insert(1, color);
		}
		// Green logic. Middle children are ill-behaved.
		else if(colors.Contains(Color.red)){
			colors.Insert(2, color);
		}
		else {
			colors.Insert(1, color);
		}
	}
	
	public void SetColorPainted(Color color) {
		colorPainted = color;
		gameObject.renderer.material.color = color;
	}
	
	public static GameObject MakePlayer(int x, int y, int health) {
		GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
		player.transform.position = new Vector3(x, y, 0.0f);
		Player script = player.AddComponent<Player>();
		script.gridCoords = new Vector2(x, y);
		script.health = health;
		GameManager.floor.Add(player, x, y);
		return player;
	}
}
