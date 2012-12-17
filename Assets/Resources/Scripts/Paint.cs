using UnityEngine;
using System;
using System.Collections;

public class Paint : MonoBehaviour, IColor {
	public Grid grid;

	public Vector2 gridCoords;
	public Color colorPainted { get; set;}
	public float lastPickedUp;
	public float respawnTime;
	private bool _isEnabled;
	public bool isEnabled
	{
		get { return _isEnabled; }
		// If this isn't enabled, make it invisible.
		set {
			_isEnabled = value;
			renderer.enabled = value;
			if(!value) {
				lastPickedUp = Time.time;
			}
		}
	}
	
	void Update() {
		if(isEnabled == false && Time.time > lastPickedUp + respawnTime) {
			isEnabled = true;
		}
		if(isEnabled) {
			GameObject player = grid.grid[(int)gridCoords.x, (int)gridCoords.y].objects.Find((GameObject obj) =>
												    obj.GetComponent<Player>() != null);
			if(player) {
				player.GetComponent<Player>().PickupColor(colorPainted);
				if(respawnTime == Single.PositiveInfinity)
					Destroy(gameObject);
				else
					isEnabled = false;
			}
		}
	}
	
	void OnDisable() {
		grid.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
	
	public static GameObject MakePaint(Grid grid, int x, int y, Color color, float respawnTime) {
		GameObject paint = GameObject.CreatePrimitive(PrimitiveType.Plane);
		paint.transform.Rotate(-90, 0, 0);
		paint.name = "Paint";
		paint.transform.localScale = new Vector3(0.065f, 0.065f, 0.065f);
		paint.transform.position = new Vector3(x, y, -0.25f);
		paint.renderer.material.mainTexture = Resources.Load("Textures/Paint") as Texture;
		paint.renderer.material.shader = Shader.Find("Transparent/Diffuse");					 
		paint.renderer.material.color = color;
		Paint script = paint.AddComponent<Paint>();
		script.gridCoords = new Vector2(x, y);
		script.colorPainted = color;
		script.isEnabled = true;
		script.respawnTime = respawnTime;
		script.lastPickedUp = Time.time - respawnTime;
		script.grid = grid;
		return paint;
	}
}
