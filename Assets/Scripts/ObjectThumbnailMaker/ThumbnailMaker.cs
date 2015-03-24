using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class ThumbnailMaker : MonoBehaviour
{
    //References

    //Public
    public Transform CanvasTransform;
    public string FileName = "NewThumb";

    //Private

    void Start()
    {
        Debug.Log("Filename: " + FileName);
    }

    public void MakeThumbnail()
    {
        CanvasTransform.gameObject.SetActive(false);
        Directory.CreateDirectory(Application.dataPath + @"\Thumbnails\");
        Application.CaptureScreenshot(Application.dataPath + @"\Thumbnails\" + "thumb" + FileName + ".png");
        Debug.Log("Made thumbnail");
    }
}
