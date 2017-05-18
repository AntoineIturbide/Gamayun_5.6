using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {

	public static Flash instance;

	Tweening.Tween<float> mFlash = new Tweening.Tween<float>(1, UnityTick.UPDATE, Easing.DynaEase.Out);

	public Image mImage;

	public Color mFlashColor = Color.white;
	public Color mTransparentColor = Color.white;

	public float firstFadeDuration;

	private void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		mFlash.change_callback += OnFlashBlend;

		MakeFlash(firstFadeDuration);
	}

	private void OnDestroy ()
	{
		mFlash.change_callback -= OnFlashBlend;
	}

	public static void MakeFlash(float duration)
	{
		instance.mFlash.current_value = 1;
		instance.mFlash.SetTarget(0);
		instance.mFlash.time_factor = 1f / duration;
	}

	public void OnFlashBlend ()
	{
		mImage.color = Color.Lerp(mTransparentColor, mFlashColor, mFlash.get_value());
	}
}
