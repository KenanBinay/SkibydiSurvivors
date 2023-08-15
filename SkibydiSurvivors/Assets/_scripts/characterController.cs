using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class characterController : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;

    public CharacterController controller;
    public GameObject fovStartPoint;
    public Animator playerAnimator;

    public float movementSpeed, rotationSpeed, idleRotationSpeed, maxAngle = 90;
    public static bool idle;
    public bool useAutoShooting;

    void Start()
    {
        playerAnimator = controller.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        var movementDirection = new Vector3(joystick.Direction.x, 0.0f, joystick.Direction.y);

        playerAnimator.SetFloat("vertical", movementDirection.sqrMagnitude);

        if (!useAutoShooting)
        {
            if (movementDirection.sqrMagnitude > 0)
            {
                controller.SimpleMove(movementDirection * movementSpeed);

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
            controller.SimpleMove(movementDirection * movementSpeed);

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
