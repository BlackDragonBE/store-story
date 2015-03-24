using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Singleton;

    //References

    //Public
    public AudioClip ChillMusic;
    public AudioClip StoreOpenMusic;
    public AudioClip SadMusic;

    //Private

    void Awake()
    {
        Singleton = this;
    }

    public void PlayChillMusic()
    {
        GetComponent<AudioSource>().clip = ChillMusic;
        GetComponent<AudioSource>().Play();
    }

    public void PlayStoreMusic()
    {
        GetComponent<AudioSource>().clip = StoreOpenMusic;
        GetComponent<AudioSource>().Play();
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void ResumeMusic()
    {
        GetComponent<AudioSource>().Play();
    }

    public void FadeToSadMusic()
    {
        StartCoroutine(FadeToSadRoutine());

    }

    private IEnumerator FadeToSadRoutine()
    {
        GetComponent<AudioSource>().DOFade(0, 2f);
        yield return new WaitForSeconds(2.1f);
        GetComponent<AudioSource>().clip = SadMusic;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().DOFade(1, 2f);
    }
}