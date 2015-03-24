using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DOScalePingPong : MonoBehaviour
{
    //References

    //Public
    public bool StartScaleIsStartScale;

    public Vector3 StartScale;
    public Vector3 EndScale;

    public float Time = 1f;

    //Private

    private void OnEnable()
    {
        if (StartScaleIsStartScale)
        {
            StartScale = transform.localScale;
        }

        StartCoroutine(DoTween());
    }

    private IEnumerator DoTween()
    {
        while (true)
        {
            Tween tw = transform.DOScale(EndScale, Time);
            yield return tw.WaitForCompletion();
            tw = transform.DOScale(StartScale, Time);
            yield return tw.WaitForCompletion();
        }
    }
}