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

	public AkBank bank;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartLevel()
	{
		StartCoroutine(Fading());
	}

	public void Quit()
	{
		StartCoroutine(QuitFading());
	}
	IEnumerator QuitFading ()
	{
		anim.SetBool("Fade", true);
		yield return new WaitUntil(() => fadeImage.color.a == 1);
		Application.Quit();
	}
	IEnumerator Fading()
	{
		anim.SetBool("Fade", true);
		yield return new WaitUntil(() => fadeImage.color.a == 1);
		bank.UnloadBank(bank.gameObject);
		SceneManager.LoadScene(levelName);
	}
}
