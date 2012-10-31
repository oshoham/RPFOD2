using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour, IColor {
	
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
	public List<Color> colors;
	public int colorIndex;

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
	
	void Start() {
		startedMoving = Time.time;
		lastMovedHorizontal = Time.time;
		lastMovedVertical = Time.time;
		moveRate = 0.1f;
		moveSpeed = 0.2f;
		colors = new List<Color>(3);
		defaultColor = renderer.material.color;
		collider.enabled = true;
	}

	void Update() {
		if(health <= 0) {
			Destroy(gameObject);
		}
		GetKeypresses();
	}

	public void GetKeypresses() {
		// This if statement makes motion a bit less smooth but also a bit more predictable
		// and avoid situations where the player is located on top of paint in world coords
		// but not in the grid, and thus doesn't pick it up.
		// Maybe we shouldn't keep it?
		if(Time.time > endMoving) {
			if(Input.GetKey("w")) {
				if(Time.time - lastMovedVertical > moveRate) {
					dir = new Vector2(0, 1);
					Move(dir);
					lastMovedVertical = Time.time;
				}
			}
			if(Input.GetKey("a")) {
				if(Time.time - lastMovedHorizontal > moveRate) {
					dir = new Vector2(-1, 0);
					Move(dir);
					lastMovedHorizontal = Time.time;
				}
			}
			if(Input.GetKey("s")) {
				if(Time.time - lastMovedVertical > moveRate) {
					dir = new Vector2(0, -1);
					Move(dir);
					lastMovedVertical = Time.time;
				}
			}
			if(Input.GetKey("d")) {
				if(Time.time - lastMovedHorizontal > moveRate) {
					dir = new Vector2(1, 0);
					Move(dir);
					lastMovedHorizontal = Time.time;
				}
			}
		}
		if(Input.GetKeyDown("1")) {
			setColorPainted(defaultColor);
		}
		if(Input.GetKeyDown("2") && colors.Contains(Color.red)) {
			setColorPainted(Color.red);
		}
		if(Input.GetKeyDown("3") && colors.Contains(Color.green)) {
			setColorPainted(Color.green);
		}
		if(Input.GetKeyDown("4") && colors.Contains(Color.blue)) {
			setColorPainted(Color.blue);
		}
		if(Input.GetKeyDown("space") && colors.Count > 0)
		{
			Paintball.MakePaintball(transform.position, dir, colorShooting, gameObject); 
		}
		AnimateMotion();
	}

	/*
	 * Increments the index which we're at in the colors list, and sets the
	 * current color painted to that color.
	 */
	public void CycleColorPainted() {
		if(colors.Count > 0) {
			setColorPainted(colors[++colorIndex % colors.Count]);
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
			newPosition = new Vector3(gridCoords.x, gridCoords.y, 0);
		}
	}
	
	/*
	 * For smooth motion animation.
	 */
	public void AnimateMotion() {
		if(Time.time > endMoving) {
			return;
		}
		float time = (Time.time - startedMoving)/moveSpeed;
		transform.position = Vector3.Lerp(oldPosition, newPosition, time);
		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,
							     Camera.main.transform.position.z);
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
		if(colors.Count == 0) {
			colorShooting = color;
		}
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
	
	public void setColorPainted(Color color) {
		colorPainted = color;
		gameObject.renderer.material.color = color;
	}
	
	public static GameObject MakePlayer(int x, int y, int health) {
		GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
		player.name = "Player";
		player.transform.position = new Vector3(x, y, 0.0f);
		Player script = player.AddComponent<Player>();
		script.gridCoords = new Vector2(x, y);
		script.health = health;
		GameManager.floor.Add(player, x, y);
		return player;
	}
}
