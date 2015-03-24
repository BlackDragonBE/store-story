using UnityEngine;
using System.Collections;

public class Cashier : MonoBehaviour
{
    //References

    //Public
    public Animator Animator;

    //Private

    void Awake()
    {
        Animator.SetBool("Walking",false);
    }

    void OnEnable()
    {
        Animator.SetBool("Walking", false);
    }
}
