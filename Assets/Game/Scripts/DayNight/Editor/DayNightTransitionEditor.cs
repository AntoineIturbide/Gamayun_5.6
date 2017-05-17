using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Tweening;

[CustomEditor(typeof(DayNightTransition))]
[CanEditMultipleObjects]
public class DayNightTransitionEditor : Editor
{
    private DayNightTransition Target()
    {
        return (DayNightTransition)target;
    }

    private void OnEnable()
    {
        var t = Target();

        t.mDayNight.change_callback += Repaint;
    }

    private void OnDisable()
    {
        var t = Target();

        t.mDayNight.change_callback -= Repaint;
    }

    public override void OnInspectorGUI()
    {
        var t = Target();

        serializedObject.Update();

        DrawDefaultInspector();

        DrawDayNightButtons();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDayNightButtons()
    {
        var t = Target();

        //GUILayout.Label(t.mDayNight.get_value().ToString());
        GUILayout.HorizontalSlider(t.mDayNight.get_value(), 0, 1);

        if (t.mDayNight.target_value < 0.5f)
        {
            if(GUILayout.Button("Transition to day"))
            {
                t.StartTransitioningToDay();
            }
        }
        else
        {
            if (GUILayout.Button("Transition to night"))
            {
                t.StartTransitioningToNight();
            }
        }
    }

    private void OnSceneGUI()
    {   
        var t = Target();
    }
}
