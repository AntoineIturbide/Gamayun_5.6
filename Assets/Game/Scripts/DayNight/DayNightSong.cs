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
		AkSoundEngine.SetState("MUSIC", "NIGHT");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
		{
			StartCoroutine(PlayTransition());
		}
	}

	public void SetTransition()
	{
		AkSoundEngine.SetState("MUSIC", "TRANSITION");
	}

	public void SetDayTime ()
	{
		AkSoundEngine.SetState("MUSIC", "DAY");
	}

	public IEnumerator PlayTransition()
	{
		AkSoundEngine.PostEvent("Stop" + night, gameObject);
		AkSoundEngine.PostEvent("Play" + transition, gameObject);

		yield return new WaitForSeconds(tDuration);

		AkSoundEngine.PostEvent("Stop" + transition, gameObject);
		AkSoundEngine.PostEvent("Play" + day, gameObject);

	}
}
