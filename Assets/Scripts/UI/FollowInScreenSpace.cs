using UnityEngine;

public class FollowInScreenSpace : MonoBehaviour
{
    //References

    //Public
    public Transform FollowTarget;
    public Vector3 Offset = Vector3.zero;
    //Private

    void LateUpdate()
    {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(FollowTarget.position + Offset);
    }
}