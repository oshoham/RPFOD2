using UnityEngine;
using System;

public class Laser : MonoBehaviour {
	
	public int damageDealt;
	public Vector2 dir;
	public RaycastHit hit;

	void Start() {

	}

	void Update() {

	}

	public void Hit(GameObject obj) {
		if(obj.GetComponent<Player>() != null) {
			obj.GetComponent<Player>().health -= damageDealt;
			//Destroy(gameObject);
		}
		else if(obj.GetComponent<Robot>() != null) {
			obj.GetComponent<Robot>().health -= damageDealt;
			//Destroy(gameObject);
		}
		else if(obj.GetComponent<DestructibleWall>() != null) {
			obj.GetComponent<DestructibleWall>().health -= damageDealt;
			//Destroy(gameObject);
		}
		else if(obj.GetComponent<ExplosiveCrate>() != null) {
			obj.GetComponent<ExplosiveCrate>().health -= damageDealt;
			//Destroy(gameObject);
		}
		/*else if(obj.GetComponent<Wall>() != null) {
			Destroy(gameObject);
		}*/
	}

	public static GameObject MakeLaser(int damage, Vector3 pos, Vector2 dir, RaycastHit hit, Color robotLaserColor) {
		GameObject laser = new GameObject("Laser");
		LineRenderer line = laser.AddComponent<LineRenderer>();
		line.SetVertexCount(2);
		line.SetColors(Color.red, Color.red);
		line.SetWidth(0.2f, 0.2f);
		line.SetPosition(0, pos);
		GameObject obj = hit.transform.gameObject;
		Vector2 hitPos = pos;
		if(obj.GetComponent<Player>() != null)
			hitPos = obj.GetComponent<Player>().gridCoords;
		else if(obj.GetComponent<Robot>() != null)
			hitPos = obj.GetComponent<Robot>().gridCoords;
		else if(obj.GetComponent<DestructibleWall>() != null)
			hitPos = obj.GetComponent<DestructibleWall>().gridCoords;
		else if(obj.GetComponent<ExplosiveCrate>() != null)
			hitPos = obj.GetComponent<ExplosiveCrate>().gridCoords;
		line.SetPosition(1, hitPos);
		Laser script = laser.AddComponent<Laser>();
		script.dir = dir;
		script.damageDealt = damage;
		Destroy(laser, 0.5f);
		script.Hit(obj);
		return laser;
	}

}
