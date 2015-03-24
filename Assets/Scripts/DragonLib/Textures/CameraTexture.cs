using UnityEngine;

public class CameraTexture : MonoBehaviour
{

    /*
    Credit to Ocular Cash – https://www.youtube.com/watch?v=ywdwckPWpV8

    Place this script on a camera, create a new material and assign it in the cameras inspector.
    Then assign the new material to any object.
     * 
     * Warning: absolutely kills framerate
    */

    static Texture2D renderedCameraTexture; // Allows global access to this by scripts

    public Material materialCameraView;

    void Awake() // This happens once only
    {
        renderedCameraTexture = new Texture2D(Screen.width, Screen.height); // Creates a blank texture at screen size

        materialCameraView.mainTexture = renderedCameraTexture; // Connects this 2D texture to a material
    }

    void OnPostRender() // This happens every frame
    {
        renderedCameraTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0); // Reads screen after all rendering

        renderedCameraTexture.Apply(); // Paste the image
    }
}
