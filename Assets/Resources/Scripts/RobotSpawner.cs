using UnityEngine;

public class RobotSpawner : MonoBehaviour, IColor {
	
	public Grid grid;
	public int health;
	public Vector2 gridCoords;
	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			renderer.material.color = value;
			_colorPainted = value;
		}
	}
	public float spawnRate;
	public float lastSpawned;
	public Vector2 spawnDirection;
	
	// Stuff for the Robot to be spawned.
	public float robotSpeed;
	public int robotDamageDealt;
	public int robotHealth;
	public int robotForwardRange;
	public int robotSideRange;
	public Vector2 robotMovementDirection;
	public Color robotColorVisible;
	public Vector2 robotFireDirection;
	public RotationMatrix robotRotation;
	public float robotFireRate;
	public Color robotColorPainted;
	
	void Update() {
		Vector2 robotCoords = spawnDirection + gridCoords;
		print(robotCoords + " " + lastSpawned + " " + spawnRate + " " + spawnDirection);
		if(Time.timeScale > 0 && Time.time > lastSpawned + spawnRate && !grid.Check(robotCoords)) {
			lastSpawned = Time.time;
			Spawn(robotCoords);
		}
	}
	
	public void Spawn(Vector2 robotCoords) {
		print("spawning");
		grid.Add(Robot.MakeRobot(grid, (int)robotCoords.x, (int)robotCoords.y, robotSpeed, robotFireRate,
					 robotDamageDealt, robotHealth, robotForwardRange, robotSideRange,
					 robotMovementDirection, robotColorVisible, robotColorPainted,
					 robotFireDirection, robotRotation),
			 (int)robotCoords.x, (int)robotCoords.y);
	}
	
	void OnDisable() {
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}

	public static GameObject MakeRobotSpawner(Grid grid, int x, int y, int health, float spawnRate,
						  Color colorPainted, float robotSpeed, int robotDamageDealt,
						  int robotHealth, int robotForwardRange, int robotSideRange,
						  Vector2 robotMovementDirection, Color robotColorVisible,
						  Vector2 robotFireDirection, RotationMatrix robotRotation,
						  float robotFireRate, Color robotColorPainted,
						  Vector2 spawnDirection) {
		GameObject spawner = GameObject.CreatePrimitive(PrimitiveType.Cube);
		spawner.name = "Robot Spawner";
		spawner.transform.position = new Vector3(x, y, -0.5f);
		spawner.renderer.material.mainTexture = Resources.Load("Textures/robotspawner") as Texture;
		spawner.renderer.material.color = Color.white;
		spawner.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		RobotSpawner script = spawner.AddComponent<RobotSpawner>();
		script.grid = grid;
		script.gridCoords = new Vector2(x, y);
		script.health = health;
		script.spawnRate = spawnRate;
		script.colorPainted = colorPainted;
		script.robotSpeed = robotSpeed;
		script.robotDamageDealt = robotDamageDealt;
		script.robotHealth = robotHealth;
		script.robotForwardRange = robotForwardRange;
		script.robotSideRange = robotSideRange;
		script.robotMovementDirection = robotMovementDirection;
		script.robotColorVisible = robotColorVisible;
		script.robotFireDirection = robotFireDirection;
		script.robotRotation = robotRotation;
		script.robotFireRate = robotFireRate;
		script.robotColorPainted = robotColorPainted;
		script.spawnDirection = spawnDirection;
		return spawner;
	}
	
}