using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : MonoBehaviour, IColor {

	public int health;
	public float moveSpeed;
	public Vector2 gridCoords;
	public int damageDealt;
	public int range;
	public Vector2 movementDirection;
	public Vector2 fireDirection;
	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			renderer.material.color = value;
			_colorPainted = value;
		}
	}
	public Color colorVisible;

	public float startedMoving;
	public float endMoving;
	public Vector3 oldPosition;
	public Vector3 newPosition;

	public float lastFired;
	public float fireRate;
	
	void Start() {
		lastFired = Time.time;
		fireRate = 2.0f;
		collider.enabled = true;
	}

	void Update() {
		if(health <= 0) {
			Destroy(gameObject);
		}
		if(Time.time  > lastFired + fireRate) {
			Fire();
		}
		if(Time.time > endMoving) {
			Move(movementDirection);
		}
		AnimateMotion();
	}

	void Move(Vector2 coords) {
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject)) {
			gridCoords += coords;
			startedMoving = Time.time;
			endMoving = startedMoving + moveSpeed;
			oldPosition = transform.position;
			newPosition = new Vector3(gridCoords.x, gridCoords.y, 0);
		}
		else { // Face the other direction if we hit something
			if(movementDirection == fireDirection) {
				fireDirection *= -1.0f;
			}
			movementDirection *= -1.0f;
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
	}

	public void Fire() {
		List<GameObject> objects = GameManager.floor.CheckLine(gridCoords, gridCoords + fireDirection*range);
		List<GameObject> visibles = objects.FindAll((GameObject obj) => {
				Player p = obj.GetComponent<Player>();
				// watch this if statement -- without parens around the || part,
				// we get a null pointer and unity is sad
				if(p != null && (p.colorPainted == colorVisible ||
						 p.colorPainted == p.defaultColor)) {
					return true;
				}
				Wall w = obj.GetComponent<Wall>();
				if(w != null && w.colorPainted == colorVisible) {
					return true;
				}
				return false;
			});
		// Obviously this should fire, but we've not worked out projectiles yet.
		if(visibles.Count > 0) {
			Bullet.MakeBullet(damageDealt, transform.position, fireDirection, gameObject);
		}
		lastFired = Time.time;
	}

	public static GameObject MakeRobot(int x, int y, float speed, int damage, int health,
					   int range, Vector2 movementDirection, Color colorVisible,
					   Vector2 fireDirection = default(Vector3)) {
		if(fireDirection == default(Vector2)) {
			fireDirection = movementDirection;
		}
		GameObject robot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		robot.name = "Robot";
		GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
		indicator.name = "indicator";
		indicator.transform.parent = robot.transform;
		indicator.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		indicator.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);
		indicator.renderer.material.color = colorVisible;
		Robot script = robot.AddComponent<Robot>();
		robot.transform.position = new Vector3(x, y, 0.0f);
		script.oldPosition = robot.transform.position;
		script.range = range;
		script.gridCoords = new Vector2(x, y);
		script.moveSpeed = speed;
		script.damageDealt = damage;
		script.movementDirection = movementDirection;
		script.fireDirection = fireDirection;
		script.colorVisible = colorVisible;
		script.health = health;
		GameManager.floor.Add(robot, x, y);
		return robot;
	}
}
