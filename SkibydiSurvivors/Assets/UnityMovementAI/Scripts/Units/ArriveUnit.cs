using UnityEngine;

namespace UnityMovementAI
{
    public class ArriveUnit : MonoBehaviour
    {
        public Transform target;

        public Vector3 targetPosition;

        SteeringBasics steeringBasics;

        void Start()
        {
            steeringBasics = GetComponent<SteeringBasics>();
            target = gameController.instance.playerObject.transform;
        }

        void FixedUpdate()
        {
            if (target != null) targetPosition = target.position;
            Vector3 accel = steeringBasics.Arrive(targetPosition);

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();
        }

        public void setTarget(Transform player)
        {
            target = player;
        }
    }
}