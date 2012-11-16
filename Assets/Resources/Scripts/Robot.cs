using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Robot : MonoBehaviour, IColor {
	public Grid grid;

	public int health;
	public float moveSpeed;
	public RotationMatrix rotation;
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
	


	void Update() {
		
		/*
		 * Colors panels to represent vision
		 */
	
		nVision = grid.SCheckLine(gridCoords, gridCoords + fireDirection*forwardRange);
			
		List<Vector2> directions = new List<Vector2> {new Vector2(1, 0),
							      new Vector2(0, 1),
							      new Vector2(-1, 0),
							      new Vector2(0, -1)};
		foreach(Vector2 direction in directions.Where(v => v != fireDirection)) {
			nVision.AddRange(grid.SCheckLine(gridCoords, (gridCoords + (direction*sideRange))));
		}
		foreach(Square sq in oVision)
		{
			incColor(sq, false);	
		}
		foreach(Square sq in nVision) {
			incColor(sq, true);	
		}
		oVision = new List<Square>();
		oVision.AddRange(nVision);

		if(Time.timeScale == 0)
			return;

		if(health <= 0) {
			Destroy(gameObject);
		}
		Fire();
		if(Time.time > endMoving) {
			Move(movementDirection);
		}
		AnimateMotion();
	}
	
	/*
	 * Increment or decrement color
	 */
	void incColor(Square sq, bool inc) {
		if(inc)
			sq.colors[colorVisible]++;
		else
			sq.colors[colorVisible]--;
		sq.SetColor();
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
			newPosition = new Vector3(gridCoords.x, gridCoords.y, -1.0f);
		}
		else { // Turn if we hit something
			if(fireDirection == movementDirection)
				fireDirection = rotation.Rotate(fireDirection);
			movementDirection = rotation.Rotate(movementDirection);
			// if(turnsLeft) {
			// 	movementDirection = new Vector2(-movementDirection.y, movementDirection.x);
			// 	fireDirection = new Vector2(-fireDirection.y, fireDirection.x);
			// }
			// else {
			// 	if(movementDirection == fireDirection) {
			// 		fireDirection *= -1.0f;
			// 	}
			// 	movementDirection *= -1.0f;
			// }
		}

		if(movementDirection == new Vector2(1, 0))
			transform.localEulerAngles = new Vector3(0, 0, 90f);
		else if(movementDirection == new Vector2(0, 1))
			transform.localEulerAngles = new Vector3(0, 0, 180f);
		else if(movementDirection == new Vector2(-1, 0))
			transform.localEulerAngles = new Vector3(0, 0, 270f);
		else if(movementDirection == new Vector2(0, -1))
			transform.localEulerAngles = new Vector3(0, 0, 360f);
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
		List<GameObject> objects = grid.CheckLine(gridCoords, gridCoords + fireDirection*forwardRange);
		List<Vector2> directions = new List<Vector2> {new Vector2(1, 0),
							      new Vector2(0, 1),
							      new Vector2(-1, 0),
							      new Vector2(0, -1)};
		foreach(Vector2 direction in directions.Where(v => v != fireDirection)) {
			objects.AddRange(grid.CheckLine(gridCoords, (gridCoords + (direction*sideRange))));
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
				Robot r = obj.GetComponent<Robot>();
				if(r != null && (r.colorPainted == colorVisible)) {
					return true;
				}
				return false;
			});
		if(visibles.Count > 0) {
			isMoving = false;
			GameObject target = visibles[0];
			Vector2 lookdir = new Vector2(0, 0);
			if(target.GetComponent<Player>() != null)
				lookdir = target.GetComponent<Player>().gridCoords - gridCoords;
			else if(target.GetComponent<Robot>() != null)
				lookdir = target.GetComponent<Robot>().gridCoords - gridCoords;
			else if(target.GetComponent<Wall>() != null)
				lookdir = target.GetComponent<Wall>().gridCoords - gridCoords;
			lookdir.Normalize();
			if(lookdir == new Vector2(1, 0))
				transform.localEulerAngles = new Vector3(0, 0, 90f);
			else if(lookdir == new Vector2(0, 1))
				transform.localEulerAngles = new Vector3(0, 0, 180f);
			else if(lookdir == new Vector2(-1, 0))
				transform.localEulerAngles = new Vector3(0, 0, 270f);
			else if(lookdir == new Vector2(0, -1))
				transform.localEulerAngles = new Vector3(0, 0, 360f);
			if(Time.time > lastFired + fireRate) {
				Bullet.MakeBullet(damageDealt, new Vector3(transform.position.x, transform.position.y, .1f), (visibles[0].transform.position - transform.position).normalized, gameObject);
				lastFired = Time.time;
			}
		}
		else {
			isMoving = true;
		}
	}

	void OnDisable() {
		foreach(Square sq in oVision) {
			incColor(sq, false);	
		}
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
		WinChecker.numRobots--;
	}
	
	public static GameObject MakeRobot(Grid grid, int x, int y, float speed, int damage, int health,
					   int forwardRange, int sideRange, Vector2 movementDirection,
					   Color colorVisible, Vector2 fireDirection,
					   RotationMatrix rotation) {
		GameObject robot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		robot.name = "Robot";
		robot.renderer.material.mainTexture = Resources.Load("Textures/BlankBot") as Texture;
		robot.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		robot.renderer.material.color = Color.white;
		GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
		indicator.name = "indicator";
		indicator.renderer.material.mainTexture = Resources.Load("Textures/Indicator") as Texture;
		indicator.transform.parent = robot.transform;
		indicator.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		indicator.transform.localPosition = new Vector3(0.0f, 0.0f, -1.5f);
		indicator.renderer.material.color = colorVisible;
		Robot script = robot.AddComponent<Robot>();
		robot.transform.position = new Vector3(x, y, -0.5f);
		script.oldPosition = robot.transform.position;
		script.newPosition = robot.transform.position;
		script.forwardRange = forwardRange;
		script.sideRange = sideRange;
		script.gridCoords = new Vector2(x, y);
		script.moveSpeed = speed;
		script.damageDealt = damage;
		script.movementDirection = movementDirection;
		script.fireDirection = fireDirection;
		script.colorVisible = colorVisible;
		script.health = health;
		script.rotation = rotation;
		script.oVision = new List<Square>();
		script.lastFired = Time.time;
		script.fireRate = 2.0f;
		script.collider.enabled = true;
		script.grid = grid;
		if(script.movementDirection == new Vector2(1, 0))
			script.transform.localEulerAngles = new Vector3(0, 0, 90f);
		else if(script.movementDirection == new Vector2(0, 1))
			script.transform.localEulerAngles = new Vector3(0, 0, 180f);
		else if(script.movementDirection == new Vector2(-1, 0))
			script.transform.localEulerAngles = new Vector3(0, 0, 270f);
		else if(script.movementDirection == new Vector2(0, -1))
			script.transform.localEulerAngles = new Vector3(0, 0, 360f);
		WinChecker.numRobots++;
		return robot;
	}
}
