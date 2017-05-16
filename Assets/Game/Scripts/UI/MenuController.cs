using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public string levelName;

	public Image fadeImage;
	public Animator anim;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartLevel()
	{
		StartCoroutine(Fading());
		Debug.Log("CANCER");
	}

	IEnumerator Fading()
	{
		Debug.Log("EBOLA");
		anim.SetBool("Fade", true);
		yield return new WaitUntil(() => fadeImage.color.a == 1);
		SceneManager.LoadScene(levelName);
	}
}
