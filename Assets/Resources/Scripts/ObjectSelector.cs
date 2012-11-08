using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectSelector : MonoBehaviour {

	public ObjectType objectType;

	void OnMouseDown() {
		// transform.localScale = new Vector3(0.3f, 1.0f, 0.3f);
		LevelEditor.objectToBeCreated = objectType;
	}
}
