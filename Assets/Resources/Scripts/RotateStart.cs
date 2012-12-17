using UnityEngine;
using System.Collections;

public class RotateStart : MonoBehaviour {
	private float rotSpeed = 1;
	private float fadeSpeed = 0.001F;
	private Color alphaColor = new Vector4(0.5F,0.5F,0.5F,0);

	void Start(){
		transform.renderer.material.SetColor("_TintColor", alphaColor);
	}
	
	void Update(){		
		if(transform.renderer.material.GetColor("_TintColor").a < 0.3F)	{
			print(transform.renderer.material.GetColor("_TintColor").a);
			transform.renderer.material.SetColor("_TintColor", alphaColor);
		}			      
				
		
//		if(rotSpeed%60 == 0) 
//			transform.rotation = Random.rotation;
		alphaColor.a += fadeSpeed;	
		rotSpeed++;
		
			            
	}
}