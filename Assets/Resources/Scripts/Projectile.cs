using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Projectile : MonoBehaviour {

	public Vector2 dir;
	public GameObject cameFrom;
	public float moveSpeed;
	public float lifeTime;
	
	void Update() {
		transform.Translate(dir * moveSpeed * Time.deltaTime);
		CheckPosition();
	}
	
	public abstract void Hit(GameObject obj);
	
	/*
	 * Check if we've hit anything.
	 */
	public void CheckPosition() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, .1f);
		foreach(Collider collider in colliders) {
			GameObject obj = collider.gameObject;
			if(obj == cameFrom)
				continue;
			Hit(obj);
		}
	}
}
