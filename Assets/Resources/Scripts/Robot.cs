using UnityEngine;
using System;
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
	public static AudioSource lasersource = new AudioSource();
	public static AudioClip lasersound;	

	void Start()
	{
		
	}

	void Update() {
		
		/*
		 * Colors panels to represent vision
		 */
	
		nVision = grid.SCheckLine(gridCoords, gridCoords + fireDirection*forwardRange);
		nVision.Add(grid.grid[(int)gridCoords.x, (int)gridCoords.y]);
			
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
		if(Time.time > endMoving && moveSpeed > 0) {
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
	
	/*
	 * For smooth motion animation.
	 */
	public void AnimateMotion() {
		if(Time.time > endMoving || moveSpeed == 0) {
			return;
		}
		float time = (Time.time - startedMoving)/moveSpeed + .1f;
		if(moveSpeed >= 1) { // If this is a slower bot, use smooth motion
			transform.position = CubicInterpolate(oldPosition, newPosition, time);
		}
		else {
			transform.position = Vector3.Lerp(oldPosition, newPosition, time);
		}
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
				Robot r = obj.GetComponent<Robot>();
				if(r != null && (r.colorPainted == colorVisible)) {
					return true;
				}
				DestructibleWall d = obj.GetComponent<DestructibleWall>();
				if(d != null && (d.colorPainted == colorVisible)) {
					return true;
				}
				ExplosiveCrate e = obj.GetComponent<ExplosiveCrate>();
				if(e != null && (e.colorPainted == colorVisible)) {
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
			else if(target.GetComponent<DestructibleWall>() != null)
				lookdir = target.GetComponent<DestructibleWall>().gridCoords - gridCoords;
			else if(target.GetComponent<ExplosiveCrate>() != null)
				lookdir = target.GetComponent<ExplosiveCrate>().gridCoords - gridCoords;
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
				RaycastHit hit;
				if(Physics.Raycast(transform.position, (visibles[0].transform.position - transform.position).normalized, out hit)) {
					bool shouldShoot = hit.transform.gameObject.Equals(visibles[0]);
					if(!shouldShoot) {
						if(hit.transform.gameObject.GetComponent<Robot>() != null &&
						   visibles[0].GetComponent<Robot>() != null)
							shouldShoot = true;
					}
					if(shouldShoot) {
						
						lastFired = Time.time;
						Laser.MakeLaser(damageDealt, transform.position, (visibles[0].transform.position - transform.position).normalized, hit, colorVisible, this, Color.red);
						lasersound = Resources.Load("Audio/Effects/robotshot") as AudioClip;
						lasersource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
						lasersource.clip = lasersound;
						lasersource.Play();
					}
				}
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
	
	public static GameObject MakeRobot(Grid grid, int x, int y, float speed, float fireRate,
					   int damage, int health, int forwardRange,
					   int sideRange, Vector2 movementDirection,
					   Color colorVisible, Color colorPainted,
					   Vector2 fireDirection, RotationMatrix rotation) {
		GameObject robot = GameObject.CreatePrimitive(PrimitiveType.Cube);
		robot.name = "Robot";
		robot.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		robot.renderer.material.mainTexture = Resources.Load("Textures/BlankBot") as Texture;
		robot.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		robot.renderer.material.color = Color.white;
		GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
		indicator.name = "indicator";
		indicator.renderer.material.mainTexture = Resources.Load("Textures/Indicator") as Texture;
		indicator.transform.parent = robot.transform;
		indicator.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		indicator.transform.localPosition = new Vector3(0.0f, 0.0f, -1.0f);
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
		script.colorPainted = colorPainted;
		script.health = health;
		script.rotation = rotation;
		script.oVision = new List<Square>();
		script.lastFired = Time.time;
		script.fireRate = fireRate;
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
		print("Win checker robot count is " + WinChecker.numRobots);
		return robot;
	}
}
