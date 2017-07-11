using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    [SelectionBase]
	public class CharacterMovement : MonoBehaviour
	{
        [Header("Capsule Collider Settings")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.03f;

        [Header("Movement Properties")]
        [SerializeField] float movingTurnSpeed = 1000;
        [SerializeField] float stationaryTurnSpeed = 800;
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float animSpeedMultiplier = 1.5f;
        [SerializeField] float moveThreshold = 1f;
        [SerializeField] float animatorDampingTime = 0.1f;
		[SerializeField] float stoppingDistance = 1f;

        Rigidbody myRigidbody;
        Animator animator;
        float turnAmount;
        float forwardAmount;
		NavMeshAgent agent = null;

		void Start()
		{
            AddRequiredComponents();

			animator = GetComponent<Animator>();
			myRigidbody = GetComponent<Rigidbody>();
			myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

			agent = GetComponent<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updatePosition = true;
			agent.stoppingDistance = stoppingDistance;
        }

        void AddRequiredComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;
        }

		void Update()
		{
			if (agent.remainingDistance > agent.stoppingDistance)
			{
				Move(agent.desiredVelocity);
			}
			else
			{
				Move(Vector3.zero);
			}
		}

		public void SetDestination(Vector3 worldPos)
		{
			agent.destination = worldPos;
		}

        public float GetAnimSpeedMultiplier()
        {
            return animSpeedMultiplier;
        }

        private void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator(); // send input and other state parameters to the animator
        }

        private void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired direction
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
		{
			animator.SetFloat("Forward", forwardAmount, animatorDampingTime, Time.deltaTime);
			animator.SetFloat("Turn", turnAmount, animatorDampingTime, Time.deltaTime); 
			animator.speed = animSpeedMultiplier;
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
		}

		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (Time.deltaTime > 0)
			{
				Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = myRigidbody.velocity.y;
				myRigidbody.velocity = v;
			}
		}
	}
}
