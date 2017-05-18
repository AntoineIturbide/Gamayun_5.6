﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaliseTrigger : MonoBehaviour
{
	public static System.Action eOnBaliseTriggered;

	public GameObject particleSpawn;
	public string eventName;
	public Transform spawnLocation;

	public GoToNight goToNight;

	bool isDay;
	// Use this for initialization
	void Start ()
	{
		isDay = false;
	}

	private void OnTriggerEnter (Collider other)
	{
		if(other.tag == "Player")
		{
			GameObject particle = Instantiate(particleSpawn, transform.position + transform.up * 30f, transform.rotation);
			Destroy(particle, 3f);

			AkSoundEngine.PostEvent(eventName, gameObject);
			if (!isDay)
			{
				isDay = true;
				if(eOnBaliseTriggered != null)
				{
					eOnBaliseTriggered();
				}
				goToNight.isDay = isDay;
			}
			
		}
	}
}
