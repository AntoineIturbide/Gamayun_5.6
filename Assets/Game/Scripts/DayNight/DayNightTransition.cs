using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class DayNightTransition : MonoBehaviour {

    [System.NonSerialized]
    public Tween<float> mDayNight = new Tween<float>(
        1,
        UnityTick.UPDATE,
        Easing.DynaEase.Out
        );
    
    const int cLevels = 6;
    const float cStep = 1f / cLevels;

    public float mTransitionTime = 1;

    public Texture2D maqueDeVie;

	public GameObject dayCam;
	public GameObject nightCam;

	public Lutify lutD;
	public Lutify lutN1;
	public Lutify lutN2;

	public Material matD;

	public Color colD;
	public Color colN;

	public Light light;

	[Range(0,1)]
    public float mGamayun = 0;

	[Range(0, 1)]
	public float almostThere = 0.75f;
	public float mDayNightPostFxTarget = 0;
	public float mDayNightPostFxCurrent = 0;
	public Tween<float> mDayNightPostFx = new Tween<float>(
		1,
		UnityTick.UPDATE,
		Easing.DynaEase.Out,
		1f/2f
		);

	private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        Close();
    }

    private void OnValidate()
    {

        Shader.SetGlobalFloat("_Gamayun", mGamayun);

        if (mDayNight != null)
        {
            Shader.SetGlobalFloat("_GamayunFlash", mDayNight.get_value());
        }
        Shader.SetGlobalTexture("_GamayunTex", maqueDeVie);

		mDayNightPostFx.SetTarget(mDayNightPostFxTarget);

	}

    private void Init()
    {
        mDayNight = new Tween<float>(
        0,
        UnityTick.UPDATE,
        Easing.DynaEase.Linear,
        1f
        );

        mDayNight.change_callback += OnDayNightChange;

        Shader.SetGlobalFloat("_GamayunFlash", 2.2f);
        Shader.SetGlobalTexture("_GamayunTex", maqueDeVie);

		BaliseTrigger.eOnBaliseTriggered += PlusOne;
		mDayNightPostFx.change_callback += OnDayNightPostFxChange;

	}

    private void Close()
    {
        mDayNight.change_callback -= OnDayNightChange;
		BaliseTrigger.eOnBaliseTriggered -= PlusOne;
		mDayNightPostFx.change_callback -= OnDayNightPostFxChange;
	}

    private void OnDayNightChange()
    {
        Shader.SetGlobalFloat("_Gamayun", mDayNight.get_value());
    }

    public void StartTransitioningToDay()
    {
        mDayNight.SetTarget(1f);
		mDayNight.time_factor = 1f / mTransitionTime;
    }
    public void StartTransitioningToNight()
    {
		mDayNight.SetTarget(0f);
		mDayNight.time_factor = 1f / mTransitionTime;
    }

    public void PlusOne()
	{
		Flash.MakeFlash(0.25f);
		mDayNight.SetTarget(Mathf.Clamp01(mDayNight.target_value + cStep));
        mDayNight.time_factor = 1f / mTransitionTime;
		mDayNightPostFxCurrent += almostThere / 6f;
		if (mDayNightPostFxCurrent > almostThere - 0.01f)
			mDayNightPostFxCurrent = 1f;
		SetDay();
	}
    public void MinusOne()
    {
        mDayNight.SetTarget(Mathf.Clamp01(mDayNight.target_value - cStep));
        mDayNight.time_factor = 1f / mTransitionTime;
		mDayNightPostFxCurrent -= almostThere / 6f;
		if (mDayNightPostFxCurrent < almostThere + 0.01f)
			mDayNightPostFxCurrent = almostThere;

		SetNight();
	}

	public void SetDay ()
	{
		//dayCam.SetActive(true);
		//nightCam.SetActive(false);
		mDayNightPostFx.SetTarget(mDayNightPostFxCurrent);
	}
	public void SetNight ()
	{
		//dayCam.SetActive(false);
		//nightCam.SetActive(true);
		mDayNightPostFx.SetTarget(0);
	}

	public void OnGUI()
    {
        //GUILayout.Label("")
    }

	private void OnDayNightPostFxChange ()
	{
		float dn = mDayNightPostFx.get_value();
		lutD.Blend = dn;
		light.intensity = Mathf.Lerp(0.6f, 1f, dn);
		dn = 1 - dn;
		lutN1.Blend = dn;
		lutN2.Blend = dn;
		matD.SetColor("_Tint", Color.Lerp(colD, colN, dn));
	}
}
