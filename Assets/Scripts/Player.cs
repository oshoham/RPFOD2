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
	
	void Start() {
		startedMoving = Time.time;
		lastMovedHorizontal = Time.time;
		lastMovedVertical = Time.time;
		moveRate = 0.1f;
		moveSpeed = 0.2f;
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
		if(Input.GetKey("w")) {
			if(Time.time - lastMovedVertical > moveRate) {
				Move(new Vector2(0, 1));
				lastMovedVertical = Time.time;
			}
		}
		if(Input.GetKey("a")) {
			if(Time.time - lastMovedHorizontal > moveRate) {
				Move(new Vector2(-1, 0));
				lastMovedHorizontal = Time.time;
			}
		}
		if(Input.GetKey("s")) {
			if(Time.time - lastMovedVertical > moveRate) {
				Move(new Vector2(0, -1));
				lastMovedVertical = Time.time;
			}
		}
		if(Input.GetKey("d")) {
			if(Time.time - lastMovedHorizontal > moveRate) {
				Move(new Vector2(1, 0));
				lastMovedHorizontal = Time.time;
			}
		}
		/*if(Input.GetKeyDown("q")) {
			CycleColorPainted();
		}*/
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
		/*if(color == Color.blue) {
			colors.Add(color);
		}
		else if(color == Color.red) {
			colors.Insert(1, color);
			FixColorIndex(1);
		}
		// Green logic. Middle children are ill-behaved.
		else if(colors.Contains(Color.red)){
			colors.Insert(2, color);
			FixColorIndex(2);
		}
		else {
			colors.Insert(1, color);
			FixColorIndex(1);
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
