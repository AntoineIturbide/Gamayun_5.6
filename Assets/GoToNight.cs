using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNight : MonoBehaviour {

	[System.NonSerialized]
	public bool isDay = false;

	public DayNightTransition dnt;

	private void Start ()
	{
		isDay = false;
	}

	private void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			if (isDay)
			{
				dnt.SetDay();
			}
			else
			{
				dnt.SetNight();
			}
		}
	}

}
