using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour, IColor {

	public Vector2 gridCoords;
	public Vector2 dir;
	public Color colorPainted{ get; set; }
	public GameObject cameFrom;


	void Start() {

	}
	
	void Update() {
		//what should what type of object should I look for?
	}

	

	public void Move(Vector2 coords) {
		if(GameManager.Move(gridCoords, gridCoords + coords, gameObject))
			gridCoords += coords;
	}

	public static GameObject MakeProj(int x, int y, Vector3 pos, Vector2 dir, Color col, GameObject cameFrom) {
		GameObject proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		proj.transform.localScale = new Vector3(.0625f, .0625f, .0625f);
		proj.name = "Projectile";
		proj.transform.position = pos;
		Projectile script = proj.AddComponent<Projectile>();
		script.gridCoords = new Vector2(x, y);
		script.dir = dir;
		script.colorPainted = col;
		proj.AddComponent<Rigidbody>();
		proj.rigidbody.isKinematic = false;
		proj.rigidbody.useGravity = false;
		proj.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		proj.rigidbody.collider.isTrigger = true;
		proj.rigidbody.collider.enabled = true;
		script.cameFrom = cameFrom;
		return proj;
	}
}
