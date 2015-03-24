using UnityEngine;

public class ToggleActiveGameObject : MonoBehaviour
{
    //References

    //Public

    //Private

    public void ToggleActive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}