using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency;
    public float magnitude;
    public Vector3 direction;
    Vector3 initalPosition;
    Pickup pickup;

    void Start()
    {
        pickup = GetComponent<Pickup>();
        initalPosition = transform.position;
    }

    void Update()
    {
        if (pickup && !pickup.hasBeenCollected)
        {
            transform.position = initalPosition + direction * Mathf.Sin(Time.time * frequency)
                * magnitude;
        }
    }
}
