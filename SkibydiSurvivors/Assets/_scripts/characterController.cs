using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;

    public CharacterController controller;
    public float movementSpeed, rotationSpeed;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var movementDirection = new Vector3(joystick.Direction.x, 0.0f, joystick.Direction.y);
        controller.SimpleMove(movementDirection * movementSpeed);

        if (movementDirection.sqrMagnitude <= 0) { return; }

        var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection
            , rotationSpeed * Time.deltaTime, 0.0f);

        controller.transform.rotation = Quaternion.LookRotation(targetDirection);
    }
}
