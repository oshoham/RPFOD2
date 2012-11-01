using UnityEngine;
using System.Collections;

public class Paint : MonoBehaviour, IColor {

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
	}
	
	void OnDisable() {
		GameManager.floor.Remove(gameObject, (int)gridCoords.x, (int)gridCoords.y);
	}
	
	public static GameObject MakePaint(int x, int y, Color color, float respawnTime) {
		GameObject paint = GameObject.CreatePrimitive(PrimitiveType.Cube);
		paint.name = "Paint";
		paint.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		paint.transform.position = new Vector3(x, y, -0.25f);
		paint.renderer.material.color = color;
		GameManager.floor.Add(paint, x, y);
		Paint script = paint.AddComponent<Paint>();
		script.gridCoords = new Vector2(x, y);
		script.colorPainted = color;
		script.isEnabled = true;
		script.respawnTime = respawnTime;
		script.lastPickedUp = Time.time - respawnTime;
		return paint;
	}
}
