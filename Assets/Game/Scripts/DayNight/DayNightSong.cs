using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSong : MonoBehaviour {

	public string night = "_Music_Night_Loop";
	public string day = "_Music_Day_Loop";
	public string transition = "_Music_Transition_Loop";
	public float tDuration;
	// Use this for initialization
	void Start ()
	{
		AkSoundEngine.PostEvent("Play" + night, gameObject);
	}
	
	void Update()
	{
		if(Input.GetKeyDown (KeyCode.O))
		{
			SetTransition();
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			SetDayTime();
		}
	}
	public void SetTransition()
	{
		AkSoundEngine.PostEvent("Stop" + night, gameObject);
		AkSoundEngine.PostEvent("Play" + transition, gameObject);
	}

	public void SetDayTime ()
	{
		AkSoundEngine.PostEvent("Stop" + transition, gameObject);
		AkSoundEngine.PostEvent("Play" + day, gameObject);
	}

}
