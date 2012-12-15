using UnityEngine;
using System.Collections;

public class RotateStart : MonoBehaviour {
	private float rotSpeed = 1;

	void Update(){		
		if (rotSpeed%30 == 0) 
			transform.rotation = Random.rotation;
		rotSpeed++;
	}
}      
