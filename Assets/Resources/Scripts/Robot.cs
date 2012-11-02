using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Robot : MonoBehaviour, IColor {

	public int health;
	public float moveSpeed;
	public bool turnsLeft;
	public Vector2 gridCoords;
	public int damageDealt;
	public int forwardRange;
	public int sideRange;
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

	public List<Square> oVision;
	public List<Square> nVision;
	public float startedMoving;
	public float endMoving;
	public Vector3 oldPosition;
	public Vector3 newPosition;

	public float lastFired;
	public float fireRate;
	public bool isMoving;
	
	void Start() {
		lastFired = Time.time;
		fireRate = 2.0f;
		collider.enabled = true;
	}

	void Update() {
		
		/*
		 * Colors panels to represent vision
		 */
	
		nVision = GameManager.floor.SCheckLine(gridCoords, gridCoords + fireDirection*forwardRange);
			
		List<Vector2> directions = new List<Vector2> {new Vector2(1, 0),
							      new Vector2(0, 1),
							      new Vector2(-1, 0),
							      new Vector2(0, -1)};
		foreach(Vector2 direction in directions.Where(v => v != fireDirection)) {
			nVision.AddRange(GameManager.floor.SCheckLine(gridCoords, (gridCoords + (direction*sideRange))));
		}
		
		foreach(Square sq in oVision)
		{
			sq.colorPainted = Color.white;
		}
		foreach(Square sq in nVision) {
			sq.colorPainted = colorVisible;
		}
		oVision = new List<Square>();
		oVision.AddRange(nVision);

		if(health <= 0) {
			Destroy(gameObject);
		}
		Fire();
		if(Time.time > endMoving) {
			Move(movementDirection);
		}
		AnimateMotion();
	}

	void Move(Vector2 coords) {
		if(!isMoving) {
			return;
		}
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject)) {
			gridCoords += coords;
			startedMoving = Time.time;
			endMoving = startedMoving + moveSpeed;
			oldPosition = transform.position;
			newPosition = new Vector3(gridCoords.x, gridCoords.y, 0);
		}
		else { // Turn if we hit something
			if(turnsLeft) {
				movementDirection = new Vector2(-movementDirection.y, movementDirection.x);
				fireDirection = new Vector2(-fireDirection.y, fireDirection.x);
			}
			else {
				if(movementDirection == fireDirection) {
					fireDirection *= -1.0f;
				}
				movementDirection *= -1.0f;
			}
		}
	}

	/*
	 * For smooth motion animation.
	 */
	public void AnimateMotion() {
		if(Time.time > endMoving) {
			return;
		}
		float time = (Time.time - startedMoving)/moveSpeed + .1f;
		transform.position = Vector3.Lerp(oldPosition, newPosition, time);
	}
	
	public void Fire() {
		List<GameObject> objects = GameManager.floor.CheckLine(gridCoords, gridCoords + fireDirection*forwardRange);
		List<Vector2> directions = new List<Vector2> {new Vector2(1, 0),
							      new Vector2(0, 1),
							      new Vector2(-1, 0),
							      new Vector2(0, -1)};
		foreach(Vector2 direction in directions.Where(v => v != fireDirection)) {
			objects.AddRange(GameManager.floor.CheckLine(gridCoords, (gridCoords + (direction*sideRange))));
		}
		
		List<GameObject> visibles = objects.FindAll((GameObject obj) => {
				Player p = obj.GetComponent<Player>();
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
		if(visibles.Count > 0) {
			isMoving = false;
			if(Time.time > lastFired + fireRate) {
				Bullet.MakeBullet(damageDealt, transform.position, visibles[0].transform.position - transform.position, gameObject);
				lastFired = Time.time;
			}
		}
		else {
			isMoving = true;
		}
	}

	void OnDisable() {
		GameManager.floor.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
	
	public static GameObject MakeRobot(int x, int y, float speed, int damage, int health,
					   int forwardRange, int sideRange, Vector2 movementDirection,
					   Color colorVisible, Vector2 fireDirection = default(Vector3),
					   bool turnsLeft = false) {
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
		script.forwardRange = forwardRange;
		script.sideRange = sideRange;
		script.gridCoords = new Vector2(x, y);
		script.moveSpeed = speed;
		script.damageDealt = damage;
		script.movementDirection = movementDirection;
		script.fireDirection = fireDirection;
		script.colorVisible = colorVisible;
		script.health = health;
		script.turnsLeft = turnsLeft;
		GameManager.floor.Add(robot, x, y);
		script.nVision = GameManager.floor.SCheckLine(script.gridCoords, script.gridCoords + fireDirection*forwardRange);
		script.oVision = script.nVision;
		return robot;
	}
}