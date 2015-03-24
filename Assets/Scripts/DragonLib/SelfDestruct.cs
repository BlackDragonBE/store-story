using System.Collections;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    #region Fields

    public float TimeUntilDestruction = 1.0f;

    #endregion Fields



    #region Methods

    private void Start()
    {
        StartCoroutine(Destruct(TimeUntilDestruction));
    }

    private IEnumerator Destruct(float timeUntilDestruction)
    {
        yield return new WaitForSeconds(timeUntilDestruction);
        Destroy(gameObject);
    }

    #endregion Methods
}