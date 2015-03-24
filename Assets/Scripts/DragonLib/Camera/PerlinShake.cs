using UnityEngine;
using System.Collections;

public class PerlinShake : MonoBehaviour
{
    //References

    //Public
    public float ShakeStrength;
    public float ShakeSpeedMultiplier;

    private Vector3 _offset;
    private Vector3 _initalPosition;

    //Private

    void Start()
    {
        _initalPosition = transform.position;
        _offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    void Update()
    {
        float sampleX = Mathf.PerlinNoise(Time.time * ShakeSpeedMultiplier * _offset.x, Time.time * ShakeSpeedMultiplier * _offset.x) * ShakeStrength;
        float sampleY = Mathf.PerlinNoise(Time.time * ShakeSpeedMultiplier * _offset.y, Time.time * ShakeSpeedMultiplier * _offset.y) * ShakeStrength;
        float sampleZ = Mathf.PerlinNoise(Time.time * ShakeSpeedMultiplier * _offset.z, Time.time * ShakeSpeedMultiplier * _offset.z) * ShakeStrength;

        transform.position = _initalPosition + new Vector3(sampleX, sampleY, sampleZ);

    }
}
