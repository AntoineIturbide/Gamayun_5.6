using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaliseTrigger : MonoBehaviour
{

	public GameObject particleSpawn;
	public string eventName;
	public Transform spawnLocation;
	// Use this for initialization
	void Start ()
	{
		
	}

	private void OnTriggerEnter (Collider other)
	{
		if(other.tag == "Player")
		{
			GameObject particle = Instantiate(particleSpawn, transform.position + transform.up * 30f, transform.rotation);
			Destroy(particle, 3f);

			AkSoundEngine.PostEvent(eventName, gameObject);
		}
	}
}
