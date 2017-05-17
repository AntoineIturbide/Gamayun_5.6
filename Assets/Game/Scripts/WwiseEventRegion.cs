using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseEventRegion : MonoBehaviour
{
	public string eventName;
	public string eventNameEnd;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter (Collider other)
	{
		if(other.tag == "Player")
		{
			AkSoundEngine.PostEvent(eventName, gameObject);
		}
		
	}

	private void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			AkSoundEngine.PostEvent(eventNameEnd, gameObject);
		}
	}
}
