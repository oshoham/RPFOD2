using UnityEngine;
using System.Collections.Generic;

public class ExplosiveCrate : MonoBehaviour, IColor {

	public Grid grid;
	private Color _colorPainted;
	public Color colorPainted
	{
		get { return _colorPainted; }
		set {
			_colorPainted = value;
			gameObject.renderer.material.color = value;
		}
	}
	public Vector2 gridCoords;
	public int health;
	public int range;
	public int damageDealt;
	public List<Square> see;
	
	public AudioClip explosion;
	
	void Start() {
		see = grid.SCheckRad(range, gridCoords);
		foreach(Square sq in see)
			sq.plane.renderer.material.mainTexture = Resources.Load("Textures/ETile2") as Texture;
	}
		
	void Update() {
		see = grid.SCheckRad(range, gridCoords);
		foreach(Square sq in see)
			sq.plane.renderer.material.mainTexture = Resources.Load("Textures/ETile2") as Texture;

		if(health <= 0) {
			foreach(Square sq in see) {
				bool scrate = false;
				foreach(GameObject obj in grid.GetObjectsOfTypes(sq.loc, new List<string> {"Robot",
								"DestructibleWall", "Player"})) {
					if(obj.GetComponent<ExplosiveCrate>())
						scrate = true;
					if(scrate) {
						scrate = false;
						continue;
					}
					if(obj.GetComponent<Robot>())
						obj.GetComponent<Robot>().health = 0;
					if(obj.GetComponent<DestructibleWall>())
						obj.GetComponent<DestructibleWall>().health = 0;
					if(obj.GetComponent<Player>())
						obj.GetComponent<Player>().health = 0;
					if(obj.GetComponent<RobotSpawner>())
						obj.GetComponent<RobotSpawner>().health = 0;
				}
				AudioPlayer.PlayAudio("Audio/Effects/crateexplosion", 10);
				Destroy(gameObject);
			}
		}
	}
	
	void OnDisable() {
		foreach(Square sq in see) {
			sq.plane.renderer.material.mainTexture = Resources.Load("Textures/Tile2") as Texture;
		}
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}

	public static GameObject MakeExplosiveCrate(Grid grid, int x, int y, int health, int range, int damageDealt) {
		GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
		crate.name = "Explosive Crate";
		crate.transform.position = new Vector3(x, y, -0.5f);
		ExplosiveCrate script = crate.AddComponent<ExplosiveCrate>();
		crate.renderer.material.mainTexture = Resources.Load("Textures/explosive") as Texture;
		crate.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		script.colorPainted = Color.white;
		script.gridCoords = new Vector2(x, y);
		script.grid = grid;
		script.health = health;
		script.range = range;
		script.damageDealt = damageDealt;
		script.explosion = Resources.Load("Audio/Effects/crateexplosion") as AudioClip;
		return crate;
	}
}
