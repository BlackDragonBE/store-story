using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Singleton;

    //References

    //Public
    public bool PlaySounds = true;
    public AudioClip CashRegisterSound;
    public AudioClip ButtonClickSound;
    public AudioClip GetItemSound;
    public AudioClip NewDaySound;
    public AudioClip MessageSound;
    public AudioClip ItemPickupSound;
    public AudioClip ItemDropSound;
    public AudioClip PageFlipSound;

    //Private

    void Awake()
    {
        Singleton = this;
    }

    public void PlayCashRegisterSound()
    {
        if (!PlaySounds)
        {
            return;
        }

        AudioSource.PlayClipAtPoint(CashRegisterSound, Vector3.zero);
    }

    public void PlayButtonClickSound()
    {
        if (!PlaySounds)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(ButtonClickSound, Vector3.zero);
    }

    public void PlayGetItemSound()
    {
        if (!PlaySounds)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(GetItemSound, Vector3.zero, .5f);
    }

    public void PlayNewDaySound()
    {
        if (!PlaySounds)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(NewDaySound, Vector3.zero);
    }

    public void PlayMessageSound()
    {
        if (!PlaySounds)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(MessageSound, Vector3.zero);
    }

    public void PlayItemPickupSound()
    {
        if (!PlaySounds)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(ItemPickupSound, Vector3.zero);
    }

    public void PlayItemDropSound()
    {
        if (!PlaySounds)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(ItemDropSound, Vector3.zero);
    }

    public void PlayPageFlipSound()
    {
        if (!PlaySounds)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(PageFlipSound, Vector3.zero);
    }
}