using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PptBehaviour : MonoBehaviour {

    public Image[] slideArray;
    private int iCurrent = 0;

    private Dictionary<Image, Tweening.Tween<float>> mTweens;

    private void Start()
    {
        mTweens = new Dictionary<Image, Tweening.Tween<float>>();

        foreach (Image slide in slideArray)
        {
            Tweening.Tween<float> tween = new Tweening.Tween<float>(0, UnityTick.UPDATE, Easing.DynaEase.Out, 1f / 0.25f);
            tween.change_callback = delegate
            {
                slide.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, tween.get_value());
            };
            mTweens.Add(slide, tween);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("joystick button 1"))
        {
            Next();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("joystick button 2"))
        {
            Previews();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("joystick button 0"))
        {
            Hide();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("joystick button 3"))
        {
            Display();
        }
    }

    public void Display()
    {
        mTweens[slideArray[iCurrent]].SetTarget(1);
    }

    public void Hide()
    {
        mTweens[slideArray[iCurrent]].SetTarget(0);
    }

    public void Next()
    {
        int iNext = iCurrent + 1;
        if(iNext > slideArray.Length - 1)
        {
            iNext = slideArray.Length - 1;
            return;
        }
        Hide();
        iCurrent = iNext;
        Display();
    }

    public void Previews()
    {
        int iNext = iCurrent - 1;
        if (iNext < 0)
        {
            iNext = 0;
            return;
        }
        Hide();
        iCurrent = iNext;
        Display();
    }
}
