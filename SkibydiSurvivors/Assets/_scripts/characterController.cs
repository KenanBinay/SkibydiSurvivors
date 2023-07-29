using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class characterController : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;

    public CharacterController controller;
    public GameObject R_hand, L_hand, enemy, fovStartPoint;
    public Animator playerAnimator;

    public float movementSpeed, rotationSpeed, maxAngle = 90;

    void Start()
    {
        playerAnimator = controller.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        var movementDirection = new Vector3(joystick.Direction.x, 0.0f, joystick.Direction.y);

        controller.SimpleMove(movementDirection * movementSpeed);

        playerAnimator.SetFloat("vertical", movementDirection.sqrMagnitude);

        var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection
  , rotationSpeed * Time.deltaTime, 0.0f);

        controller.transform.rotation = Quaternion.LookRotation(targetDirection);

    }
}
