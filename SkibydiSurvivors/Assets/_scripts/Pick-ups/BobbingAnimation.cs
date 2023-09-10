using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency;
    public float magnitude;
    public Vector3 direction;
    Vector3 initalPosition;

    void Start()
    {
        initalPosition = transform.position;
    }

    void Update()
    {
        transform.position = initalPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
    }
}
