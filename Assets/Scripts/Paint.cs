using UnityEngine;
using System.Collections;

public class Paint : MonoBehaviour {

	public Vector2 gridCoords;
	public Color color;
	
	void Start() {

	}

	void Update() {
		
	}

	public static GameObject MakePaint(int x, int y, Color color) {
		GameObject paint = GameObject.CreatePrimitive(PrimitiveType.Cube);
		paint.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		paint.transform.position = new Vector3(x, y, -0.25f);
		paint.renderer.material.color = color;
		GameManager.floor.Add(paint, x, y);
		Paint script = paint.AddComponent<Paint>();
		script.gridCoords = new Vector2(x, y);
		script.color = color;
		return paint;
	}
}
