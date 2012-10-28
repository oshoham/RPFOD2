using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public int health;
	public Vector2 gridCoords;
	public Color colorPainted;
	public Color colorShooting;
	public List<Color> colors;
	public int colorIndex;
	private Color defaultColor;
	
	void Start() {
		colors = new List<Color>(3);
		//colors.Add(renderer.material.color);
		defaultColor = renderer.material.color;
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
		/*if(Input.GetKeyDown("q")) {
			CycleColorPainted();
		}*/
		if(Input.GetKeyDown("1")) {
			SetColorPainted(defaultColor);
		}
		if(Input.GetKeyDown("2") && colors.Contains(Color.red)) {
			SetColorPainted(Color.red);
		}
		if(Input.GetKeyDown("3") && colors.Contains(Color.green)) {
			SetColorPainted(Color.green);
		}
		if(Input.GetKeyDown("4") && colors.Contains(Color.blue)) {
			SetColorPainted(Color.blue);
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
	 * red -> green -> blue if we don't. Also changes the colorIndex if
	 * necessary.
	 */
	public void PickupColor(Color color) {
		if(colors.Contains(color)) {
			return;
		}
		/*if(color == Color.blue) {
			colors.Add(color);
		}
		else if(color == Color.red) {
			FixColorIndex(1);
			colors.Insert(1, color);
		}
		// Green logic. Middle children are ill-behaved.
		else if(colors.Contains(Color.red)){
			FixColorIndex(2);
			colors.Insert(2, color);
		}
		else {
			FixColorIndex(1);
			colors.Insert(1, color);
		}*/
		colors.Add(color);
	}
	
	/*
	 * Reset the color index so that the player always gets the
	 * color they expect when pressing 'q'.
	 */
	public void FixColorIndex(int pos) {
		if(colorIndex % colors.Count >= pos) {
			colorIndex++;
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
