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
			light.color = value;
			light.intensity = 1;
			light.range = 2;
			if(value == Color.red)
				renderer.material.mainTexture = Resources.Load("Textures/RedBot") as Texture;
			else if(value == Color.green)
				renderer.material.mainTexture = Resources.Load("Textures/GreenBot") as Texture;
			else if(value == Color.blue)
				renderer.material.mainTexture = Resources.Load("Textures/BlueBot") as Texture;
			else {
				renderer.material.mainTexture = Resources.Load("Textures/BlankBot") as Texture;
				light.intensity = 0;
			}
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
	
	/*
	 * Planes for indicating vision range.
	 */
	public GameObject upPlane;
	public GameObject downPlane;
	public GameObject leftPlane;
	public GameObject rightPlane;
	/*
	 * So that we don't need to scale our vision planes each frame -- that
	 * shit is SLOW.
	 */
	public int oldUpVisionRange;
	public int oldDownVisionRange;
	public int oldLeftVisionRange;
	public int oldRightVisionRange;
	
	/*
	 * Shows the Roobitt's color when painted.
	 */
	public Light light;
	
	void Update() {
		
		/*
		 * Colors panels to represent vision.
		 */
		int upVisionRange, downVisionRange, leftVisionRange, rightVisionRange;
		nVision = new List<Square>();
		RotationMatrix rot = new RotationMatrix(RotationMatrix.Rotation.Left);
		Vector2 direction = fireDirection;
		Vector2 lightDirection = new Vector2(0, 1);
		nVision.AddRange(grid.SCheckLine(gridCoords, gridCoords + forwardRange*direction, out upVisionRange));
		if(oldUpVisionRange != upVisionRange) {
			ScalePlane(upPlane, upVisionRange, lightDirection);
			oldUpVisionRange = upVisionRange;
		}
		direction = rot.Rotate(direction);
		lightDirection = rot.Rotate(lightDirection);
		nVision.AddRange(grid.SCheckLine(gridCoords, gridCoords + sideRange*direction, out leftVisionRange));
		if(oldLeftVisionRange != leftVisionRange) {
			ScalePlane(leftPlane, leftVisionRange, lightDirection);
			oldLeftVisionRange = leftVisionRange;
		}
		direction = rot.Rotate(direction);
		lightDirection = rot.Rotate(lightDirection);
		nVision.AddRange(grid.SCheckLine(gridCoords, gridCoords + sideRange*direction, out downVisionRange));
		if(oldDownVisionRange != downVisionRange) {
			ScalePlane(downPlane, downVisionRange, lightDirection);
			oldDownVisionRange = downVisionRange;
		}
		direction = rot.Rotate(direction);
		lightDirection = rot.Rotate(lightDirection);
		nVision.AddRange(grid.SCheckLine(gridCoords, gridCoords + sideRange*direction, out rightVisionRange));
		if(oldRightVisionRange != rightVisionRange) {
			ScalePlane(rightPlane, rightVisionRange, lightDirection);
			oldRightVisionRange = rightVisionRange;
		}		oVision = new List<Square>();
		oVision.AddRange(nVision);
		if(Time.timeScale == 0)
			return;
		if(health <= 0) {
			AudioSource destruct = new AudioSource();
			AudioClip s = Resources.Load("Audio/Effects/RobExpS") as AudioClip;
			AudioClip e = Resources.Load("Audio/Effects/RobExpE") as AudioClip;
			destruct = (AudioSource)this.gameObject.AddComponent(typeof(AudioSource));
			destruct.clip = s;
			destruct.Play();
			destruct.clip = e;
			destruct.Play();
			Destroy(gameObject);			
		}
		Fire();
		if(Time.time > endMoving && moveSpeed > 0) {
			Move(movementDirection);
		}
		AnimateMotion();
	}

	/*
	 * Scales our vision planes to the appropriate amount, based on how much
	 * we can actually see (if vision is blocked by a wall), and then translates
	 * it by the right amount in the given direction.
	 */
	public void ScalePlane(GameObject plane, int range, Vector2 direction) {
		plane.transform.localScale = new Vector3(range/10.0f, .1f, .1f);
		plane.transform.localPosition = (-.5f - range/2.0f)*direction;
		plane.transform.Translate(0, .4f, 0);
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
				RobotSpawner rs = obj.GetComponent<RobotSpawner>();
				if(rs != null && (rs.colorPainted == colorVisible)) {
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
			else if(target.GetComponent<RobotSpawner>() != null)
				lookdir = target.GetComponent<RobotSpawner>().gridCoords - gridCoords;
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
		//robot.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
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
		script.light = new GameObject("Light").AddComponent<Light>();
		script.light.transform.parent = robot.transform;
		script.light.transform.localPosition = new Vector3(0, 0, 0);
		script.light.type = LightType.Point;
		script.light.intensity = 1;
		script.light.range = 2;
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
		script.upPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		script.upPlane.name = "Up";
		script.upPlane.transform.parent = robot.transform;
		script.upPlane.transform.localPosition = new Vector3(0, -1, .4f);
		script.upPlane.transform.localEulerAngles = new Vector3(0, 90, 90);
		script.downPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		script.downPlane.name = "Down";
		script.downPlane.transform.parent = robot.transform;
		script.downPlane.transform.localPosition = new Vector3(0, 1, .4f);
		script.downPlane.transform.localEulerAngles = new Vector3(0, 90, 270);
		script.leftPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		script.leftPlane.name = "Left";
		script.leftPlane.transform.parent = robot.transform;
		script.leftPlane.transform.localPosition = new Vector3(1, 0, .4f);
		script.leftPlane.transform.localEulerAngles = new Vector3(90, 180, 0);
		script.rightPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
		script.rightPlane.name = "Right";
		script.rightPlane.transform.parent = robot.transform;
		script.rightPlane.transform.localPosition = new Vector3(-1, 0, .4f);
		script.rightPlane.transform.localEulerAngles = new Vector3(270, 0, 0);
		if(colorVisible == Color.red) {
			script.upPlane.renderer.material.mainTexture = Resources.Load("Textures/redlight") as Texture;
			script.downPlane.renderer.material.mainTexture = Resources.Load("Textures/redlight") as Texture;
			script.leftPlane.renderer.material.mainTexture = Resources.Load("Textures/redlight") as Texture;
			script.rightPlane.renderer.material.mainTexture = Resources.Load("Textures/redlight") as Texture;
		}
		else if(colorVisible == Color.green) {
			script.upPlane.renderer.material.mainTexture = Resources.Load("Textures/greenlight") as Texture;
			script.downPlane.renderer.material.mainTexture = Resources.Load("Textures/greenlight") as Texture;
			script.leftPlane.renderer.material.mainTexture = Resources.Load("Textures/greenlight") as Texture;
			script.rightPlane.renderer.material.mainTexture = Resources.Load("Textures/greenlight") as Texture;
		}
		else if(colorVisible == Color.blue) {
			script.upPlane.renderer.material.mainTexture = Resources.Load("Textures/bluelight") as Texture;
			script.downPlane.renderer.material.mainTexture = Resources.Load("Textures/bluelight") as Texture;
			script.leftPlane.renderer.material.mainTexture = Resources.Load("Textures/bluelight") as Texture;
			script.rightPlane.renderer.material.mainTexture = Resources.Load("Textures/bluelight") as Texture;
		}
		script.upPlane.renderer.material.shader = Shader.Find("Particles/Additive (Soft)") as Shader;
		script.downPlane.renderer.material.shader = Shader.Find("Particles/Additive (Soft)") as Shader;
		script.leftPlane.renderer.material.shader = Shader.Find("Particles/Additive (Soft)") as Shader;
		script.rightPlane.renderer.material.shader = Shader.Find("Particles/Additive (Soft)") as Shader;
		// script.upPlane.renderer.material.shader = Shader.Find("Particles/Alpha Blended") as Shader;
		// script.downPlane.renderer.material.shader = Shader.Find("Particles/Alpha Blended") as Shader;
		// script.leftPlane.renderer.material.shader = Shader.Find("Particles/Alpha Blended") as Shader;
		// script.rightPlane.renderer.material.shader = Shader.Find("Particles/Alpha Blended") as Shader;
		script.oldUpVisionRange = -1;
		script.oldDownVisionRange = -1;
		script.oldLeftVisionRange = -1;
		script.oldRightVisionRange = -1;
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
