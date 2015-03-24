using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //References

    //Public
    public Camera PerspectiveCamera;
    public Camera IsometricCamera;

    //Private

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            if (PerspectiveCamera.gameObject.activeSelf)
            {
                PerspectiveCamera.gameObject.SetActive(false);
                IsometricCamera.gameObject.SetActive(true);
            }
            else
            {
                IsometricCamera.gameObject.SetActive(false);
                PerspectiveCamera.gameObject.SetActive(true);
            }
        }
    }
}