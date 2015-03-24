using UnityEngine;

public class WebLink : MonoBehaviour
{
    public string URL;

    public void OpenUrl()
    {
        Application.OpenURL(URL);
    }
}