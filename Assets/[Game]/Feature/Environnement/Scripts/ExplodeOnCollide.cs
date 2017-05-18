using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnCollide : MonoBehaviour
{
	public GameObject particleDestroy;
	public string eventName;

	private void OnTriggerEnter (Collider other)
	{
		if(particleDestroy != null)
		{
			Instantiate(particleDestroy, transform.position, Quaternion.identity);
		}
		AkSoundEngine.PostEvent(eventName, gameObject);
		Destroy(gameObject);
	}
}
