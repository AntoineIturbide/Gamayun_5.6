using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweening;

public class DayNightTransition : MonoBehaviour {

    [System.NonSerialized]
    public Tween<float> mDayNight = new Tween<float>(
        0,
        UnityTick.UPDATE,
        Easing.DynaEase.Out
        );

	public float mTransitionTime = 1;

    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        Close();
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
    }

    private void Close()
    {
        mDayNight.change_callback -= OnDayNightChange;
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
}
