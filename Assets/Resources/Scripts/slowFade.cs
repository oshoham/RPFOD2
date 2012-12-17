using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

//fades an object

public class slowFade : MonoBehaviour {
	public float fadeLength = 5F;
	public static float fadeStarted;
	public static bool fadeIn = true;
	public static bool fadeOut = false;

	void Update(){
		renderer.material.color = Color.Lerp(new Color(0, 0, 0, fadeIn ? 1 : 0),
							 new Color(1, 1, 1, fadeIn ? 0 : 1),
							 (Time.time - fadeStarted)/fadeLength);
	}

	void OnLevelWasLoaded(int level) {
		fadeStarted = Time.time;
		fadeIn = true;
	}

}