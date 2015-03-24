using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSText : MonoBehaviour
{
    //References

    //Public
    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    //Private

    void Start()
    {
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            GetComponent<Text>().text = format;

            if (fps < 30)
                GetComponent<Text>().color = Color.yellow;
            else
                if (fps < 10)
                    GetComponent<Text>().color = Color.red;
                else
                    GetComponent<Text>().color = Color.green;
            //	DebugConsole.Log(format,level);
            timeleft = updateInterval;
            accum = 0.0F;
            frames = 0;
        }
    }
}
