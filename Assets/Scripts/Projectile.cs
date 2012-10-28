using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Vector2 gridCoords;
	public Vector2 dir;

	void Start() {

	}
	
	void Update() {
		//what should what type of object should I look for?
	}

	

	public void Move(Vector2 coords) {
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject))
			gridCoords += coords;
	}

	public static GameObject MakeProj(int x, int y, Vector3 pos, Vector2 dir) {
		GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		proj.transform.position = pos;
		Projectile script = proj.AddComponent<Projectile>();
		script.gridCoords = new Vector2(x, y);
		script.dir = dir;
		return proj;
	}
}
