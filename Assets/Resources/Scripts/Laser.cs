using UnityEngine;
using System;

public class Laser : MonoBehaviour {
	
	public int damageDealt;
	public Vector2 dir;
	public RaycastHit hit;
	public GameObject laser;
	public GameObject target;
	public Robot roobit;

	void Start() {
	}

	void Update() {
		LineRenderer line = laser.GetComponent<LineRenderer>();
		line.SetPosition(0, roobit.transform.position);
		line.SetPosition(1, target.transform.position);
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

	public static GameObject MakeLaser(int damage, Vector3 pos, Vector2 dir, RaycastHit hit, Color robotLaserColor, Robot roobit) {
		GameObject laser = new GameObject("Laser");
		LineRenderer line = laser.AddComponent<LineRenderer>();		
		line.SetVertexCount(2);
		line.SetColors(Color.blue, Color.blue);
		line.SetWidth(0.1f, 0.1f);
		line.SetPosition(0, pos);
		line.material.color = Color.red;
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
		Destroy(laser, 0.25f);
		script.Hit(obj);
		script.laser = laser;
		script.roobit = roobit;
		script.target = obj;
		return laser;
	}

}
