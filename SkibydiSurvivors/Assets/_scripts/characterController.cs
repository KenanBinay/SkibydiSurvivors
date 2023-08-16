using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class characterController : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;

    public CharacterController controller;
    public GameObject character;
    public Animator playerAnimator;

    Vector3 _moveVector, movementDirection;

    public float movementSpeed, rotationSpeed, idleRotationSpeed, maxAngle = 90;
    public static bool idle;
    public bool useAutoShooting;

    void Start()
    {
        playerAnimator = controller.GetComponentInChildren<Animator>();
    }

    void Update()
    {
         movementDirection = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        _moveVector = Vector3.zero;
        _moveVector.x = joystick.Horizontal * movementSpeed * Time.deltaTime;
        _moveVector.z = joystick.Vertical * movementSpeed * Time.deltaTime;

        playerAnimator.SetFloat("vertical", movementDirection.sqrMagnitude);

        Move();
    }

    void Move()
    {
        if (!useAutoShooting)
        {
            if (movementDirection.sqrMagnitude > 0)
            {
                controller.Move(_moveVector); // moving the chracter by using chracterController

                var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection
          , rotationSpeed * Time.deltaTime, 0.0f);

                controller.transform.rotation = Quaternion.LookRotation(targetDirection);

                idle = false;
            }
            else if (FieldOfView.nearestTarget != null && movementDirection.sqrMagnitude <= 0)
            {
                Vector3 direction = FieldOfView.nearestTarget.position - controller.transform.position;

                direction = new Vector3(direction.x, 0, direction.z);

                // Rotate the current transform to look at the enemy
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion lookAt = Quaternion.RotateTowards(
                controller.transform.rotation, targetRotation, Time.deltaTime * idleRotationSpeed);
                controller.transform.rotation = lookAt;

                idle = true;
            }
        }
        else
        {
            controller.Move(_moveVector); // moving the chracter by using chracterController

            if (FieldOfView.nearestTarget == null && movementDirection.sqrMagnitude <= 0)
            {
                idle = true;
            }
            else if (FieldOfView.nearestTarget == null && movementDirection.sqrMagnitude > 0)
            {
                var targetDirection = Vector3.RotateTowards(controller.transform.forward, movementDirection
          , rotationSpeed * Time.deltaTime, 0.0f);

                controller.transform.rotation = Quaternion.LookRotation(targetDirection);

                idle = false;
            }
            else if (FieldOfView.nearestTarget != null)
            {
                Vector3 direction = FieldOfView.nearestTarget.position - controller.transform.position;

                direction = new Vector3(direction.x, 0, direction.z);

                // Rotate the current transform to look at the enemy
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion lookAt = Quaternion.RotateTowards(
                controller.transform.rotation, targetRotation, Time.deltaTime * idleRotationSpeed);
                controller.transform.rotation = lookAt;

                idle = false;
            }
        }
    }
}
