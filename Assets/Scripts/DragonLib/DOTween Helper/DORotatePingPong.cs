using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DORotatePingPong : MonoBehaviour
{
    //References

    //Public
    public bool StartRotationIsStartRotation;

    public Vector3 StartRotation;
    public Vector3 EndRotation;

    public bool UseLocalProperties = false;
    public float Time = 1f;

    //Private

    private void Start()
    {
        if (StartRotationIsStartRotation)
        {
            if (UseLocalProperties)
            {
                StartRotation = transform.localRotation.eulerAngles;
            }
            else
            {
                StartRotation = transform.rotation.eulerAngles;
            }
        }

        StartCoroutine(DoTween());
    }

    private IEnumerator DoTween()
    {
        while (true)
        {
            Tween tw;

            if (UseLocalProperties)
            {
                tw = transform.DORotate(EndRotation, Time, RotateMode.LocalAxisAdd);
                //transform.localRotationTo(Time, EndRotation);
            }
            else
            {
                //transform.rotationTo(Time, EndRotation);
                tw = transform.DORotate(EndRotation, Time, RotateMode.WorldAxisAdd);
            }

            yield return tw.WaitForCompletion();

            if (UseLocalProperties)
            {
                tw = transform.DORotate(StartRotation, Time, RotateMode.LocalAxisAdd);
            }
            else
            {
                tw = transform.DORotate(StartRotation, Time, RotateMode.WorldAxisAdd);
            }

            yield return tw.WaitForCompletion();
        }
    }
}