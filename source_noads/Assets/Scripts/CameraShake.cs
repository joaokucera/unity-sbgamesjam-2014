using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	#region Fields

	public Vector3 originPosition;
	public Quaternion originRotation;
	public float shake_decay = 0.002f;
	public float shake_intensity;
	public float coef_shake_intensity = 0.3f;
	
	#endregion
	
	#region Methods
	
	void Update ()
	{
		if (shake_intensity > 0) 
		{  
			transform.position = originPosition + Random.insideUnitSphere * shake_intensity;  
			
			transform.rotation = new Quaternion
			(
                originRotation.x + Random.Range (-shake_intensity, shake_intensity) * .2f, 
                originRotation.y + Random.Range (-shake_intensity, shake_intensity) * .2f, 
                originRotation.z + Random.Range (-shake_intensity, shake_intensity) * .2f, 
                originRotation.w + Random.Range (-shake_intensity, shake_intensity) * .2f
			);  
			
			shake_intensity -= shake_decay; 
		}
	}
	
	public void Shake ()
	{
		originPosition = transform.position;  
		originRotation = transform.rotation;  
		shake_intensity = coef_shake_intensity;
	}
	
	public void Origin ()
	{
		originPosition = new Vector3(0, 0, -1);  
		originRotation = Quaternion.identity;  
		shake_intensity = 0;
		
		//transform.position = originPosition + Random.insideUnitSphere * shake_intensity;  
			
		transform.rotation = new Quaternion
		(
            originRotation.x + Random.Range (-shake_intensity, shake_intensity) * .2f, 
            originRotation.y + Random.Range (-shake_intensity, shake_intensity) * .2f, 
            0,//originRotation.z + Random.Range (-shake_intensity, shake_intensity) * .2f, 
            0//originRotation.w + Random.Range (-shake_intensity, shake_intensity) * .2f
		);  
	}
	
	#endregion
}
