using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamayunCamera : MonoBehaviour {

	Camera originalCam;
	Camera copyCam;

	private void Start ()
	{
		Camera.onPreRender += OnCameraPreCull;
	}

	private void OnDestroy ()
	{
		Camera.onPreRender -= OnCameraPreCull;
	}

	private void OnCameraPreCull (Camera cam)
	{
		if (!cam.Equals(originalCam))
			return;
		copyCam.CopyFrom(originalCam);
		Debug.Log(originalCam.fieldOfView);
	}

}
