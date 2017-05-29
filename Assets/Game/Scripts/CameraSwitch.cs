using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
	public Camera avatarCam;
	public Camera[] cameras;

	int cameraIndex = 0;

	bool onAvatar = true;
	bool isDay = false;

	// Use this for initialization
	void Start ()
	{
		DisableAllCameras();
		avatarCam.enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown("joystick button 1"))
		{
			BrowseCameras(1);
		}
		else if(Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown("joystick button 2"))
		{
			BrowseCameras(-1);
		}
		else if(Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown("joystick button 3"))
		{
			if(isDay)
			{
				isDay = false;	
				SetTransition(1);
			}
			else
			{
				isDay = true;
				SetTransition(0);
			}
			
		}
		else if(Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown("joystick button 0"))
		{
			if(!onAvatar)
			{
				onAvatar = true;
				DisableAllCameras();
				avatarCam.enabled = true;
			}
			else
			{
				onAvatar = false;
				SetCamera(cameraIndex);
			}
			
		}
		
	}

	void BrowseCameras(int cursor)
	{
		onAvatar = false;
		cameraIndex += cursor;
		if(cameraIndex >= cameras.Length)
		{
			cameraIndex = 0;
		}
		else if(cameraIndex<0)
		{
			cameraIndex = cameras.Length-1;
		}
		Debug.Log("Camera Index " + cameraIndex);
		SetCamera(cameraIndex);
	}

	void SetTransition(int i)
	{
		Lutify[] luts = cameras[cameraIndex].gameObject.GetComponents<Lutify>();
		foreach(Lutify lut in luts)
		{
			lut.Blend = i;
		}
	}

	void SetCamera(int index)
	{
		DisableAllCameras();

		if(index < cameras.Length)
		{
			cameras[index].enabled = true;
		}
	}

	void DisableAllCameras()
	{
		avatarCam.enabled = false;
		foreach(Camera cam in cameras)
		{
			cam.enabled = false;
		}
	}
}
