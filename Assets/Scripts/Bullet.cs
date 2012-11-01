using UnityEngine;
using System;

public class Bullet : Projectile {
	
	public int damageDealt;
	
	public override void Hit(GameObject obj) {
		if(obj.GetComponent<Player>() != null) {
			obj.GetComponent<Player>().health -= damageDealt;
			Destroy(gameObject);
		}
		else if(obj.GetComponent<Wall>() != null) {
			if(obj.GetComponent<Wall>().destructible) {
				obj.GetComponent<Wall>().health -= damageDealt;
			}
			Destroy(gameObject);
		}
	}
	
	public static GameObject MakeBullet(int damage, Vector3 pos, Vector2 dir, GameObject cameFrom) {
		GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		bullet.transform.localScale = new Vector3(.25f, .25f, .25f);
		bullet.name = "Bullet";
		bullet.transform.position = pos;
		Bullet script = bullet.AddComponent<Bullet>();
		script.dir = dir;
		script.moveSpeed = 10.0f;
		Destroy(bullet, 2.0f); // life time
		script.cameFrom = cameFrom;
		script.damageDealt = damage;
		return bullet;
	}
}