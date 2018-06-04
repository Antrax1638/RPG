using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


[CustomEditor(typeof(UI_ToolTip))]
public class UI_ToolTipEditor : Editor
{
    private UI_ToolTip Target;
    protected int Tab;
    protected string[] TabNames = new string[] { "General", "Title" };

    protected virtual void OnEnable()
    {
        Target = (UI_ToolTip)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        Tab = GUILayout.Toolbar(Tab, TabNames);
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label(TabNames[Tab] + " Properties:");
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;

        switch (Tab)
        {
            case 0: General(); break;
            case 1: Title(); break;
        }

        EditorGUILayout.Space();
    }

    void General()
    {
        Target.Fade = EditorGUILayout.Toggle("Fade", Target.Fade);
        Target.FadeMin = EditorGUILayout.Slider("Fade Min", Target.FadeMin, 0, 1);
        Target.FadeMax = EditorGUILayout.Slider("Fade Max", Target.FadeMax, 0, 1);
        Target.FadeSmooth = EditorGUILayout.FloatField("Fade Smooth", Target.FadeSmooth);
    }

    void Title()
    {
        Target.Title = (Text)EditorGUILayout.ObjectField("Title", Target.Title, typeof(Text), true);
        Target.TitleText = EditorGUILayout.TextField("Title Text", Target.TitleText);

    }

}
