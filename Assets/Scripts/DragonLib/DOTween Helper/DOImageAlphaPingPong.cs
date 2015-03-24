using DG.Tweening;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DOImageAlphaPingPong : MonoBehaviour
{
    //References

    //Public
    public float StartAlpha;
    public float EndAlpha;


    public float Time = 1f;

    //Private

    private void OnEnable()
    {
        StartCoroutine(DoTween());
    }

    private IEnumerator DoTween()
    {
        while (true)
        {
            Tween tw = GetComponent<Image>().DOFade(EndAlpha, Time);
            yield return tw.WaitForCompletion();
            tw = GetComponent<Image>().DOFade(StartAlpha, Time);
            yield return tw.WaitForCompletion();
        }
    }
}
