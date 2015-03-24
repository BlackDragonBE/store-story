﻿/*************************************************************
 *          QUOTES WINDOW
 *          for Unity 3D
 *         
 * For happier coders worldwide
 * 
 * Author: Riccardo Stecca
 * www: http://www.riccardostecca.net
 * Check out the other packages by the author:
 * 
 * ODYSSEY: material pack for better planets
 * https://www.assetstore.unity3d.com/en/#!/content/7754
 * 
 * EXPLODING ASTEROIDS: well... exploding... asteroids.
 * https://www.assetstore.unity3d.com/en/#!/content/10025
 * 
 * WINDOWS POWER FUNCTIONS: to shutdown and reboot Windows
 * from inside Unity. Very useful for public installations.
 * https://www.assetstore.unity3d.com/en/#!/content/16114
 * 
 * No I don't want donations.
 * 
 * Thank you!
 * Enjoy coding!
 * RS
 * 
 * ***********************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Web;

public class QuotesWindow : EditorWindow
{

    static string url = "http://iheartquotes.com/api/v1/random?format=text&max_lines=4&max_characters=320";
    //static string url = "http://www.asciiartfarts.com/today.txt"; // might be an alternative :)
    static string fortuneText = "";

    Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Toys/Quotes")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(QuotesWindow), false, "Quotes");
        fortuneText = getFortune();
        window.Show();
    }

    static string getFortune()
    {
        System.DateTime dt = System.DateTime.UtcNow;

        string res = "";
        WWW www = new WWW(url);

        while (System.DateTime.UtcNow.Subtract(dt).TotalSeconds < 5f)
        {
            if (www.isDone)
            {
                res = www.text;
                break;
            }
        }

        if (res == "")
        {
            res = "Something went wrong.\n\t[This is not a quote]";
        }

        return res;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        if (GUILayout.Button("Fetch new one"))
        {
            fortuneText = getFortune();
        }
        EditorGUILayout.HelpBox(fortuneText, MessageType.None);
        if (GUILayout.Button("Copy to clipboard"))
        {
            EditorGUIUtility.systemCopyBuffer = fortuneText;
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void OnInspectorUpdate()
    {
        this.Repaint();
    }

}