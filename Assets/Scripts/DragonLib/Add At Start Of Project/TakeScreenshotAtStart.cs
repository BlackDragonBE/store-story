using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class TakeScreenshotAtStart : MonoBehaviour
{
    //References

    //Public
    public float MinDelayBeforeScreenshot = 1f;

    public float MaxDelayBeforeScreenshot = 1f;

    //Private

    private void Start()
    {
#if UNITY_EDITOR

        if (!Directory.Exists("screenshots"))
        {
            Directory.CreateDirectory("screenshots");
        }

        StartCoroutine(TakeScreenshot());
#endif
    }

    private IEnumerator TakeScreenshot()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(Random.Range(MinDelayBeforeScreenshot, MaxDelayBeforeScreenshot));
            Application.CaptureScreenshot("screenshots/" + "screenshot" + DateTime.Now.ToString("yyMMdd HHmm") + ".png");
        }
        //Debug.Log("Screenshot made");
    }
}