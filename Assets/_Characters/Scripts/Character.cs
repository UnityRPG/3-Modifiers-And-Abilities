using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Characters
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Capsule Collider Settings")] // a class is an alternative but hard with [SerializeField] paradigm
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.03f;

        [Header("Other Setup Settings")]
        [SerializeField]
        [Range(0f, 1.0f)]
        float audioSourceSpatialBlend = 0.5f;
        [SerializeField] RuntimeAnimatorController animatorController = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Avatar characterAvatar;

        [Header("Movement Properties")]
        [SerializeField]
        float movingTurnSpeed = 1000;
        [SerializeField] float stationaryTurnSpeed = 800;
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float animSpeedMultiplier = 1.5f;
        [SerializeField] float moveThreshold = 1.0f;
        [SerializeField] float animatorDampingTime = 0.1f;
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;
        [SerializeField] float navMeshAgentStoppingDistance = 1.3f;

        AudioSource audioSource;
        Rigidbody rigidBody;
        Animator animator;
        float turnAmount;
        float forwardAmount;
        NavMeshAgent navMeshAgent;
        bool isAlive = true;

        void Awake()
        {
            AddRequiredComponents();
        }

        void AddRequiredComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;

            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshAgentStoppingDistance;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = true;
        }

        void Update()
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        public void Kill()
        {
            isAlive = false;
        }

        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
        }

        public float GetAnimSpeedMultiplier()
        {
            return animSpeedMultiplier;
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return animatorOverrideController;
        }

         void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator(); // send input and other state parameters to the animator
        }

         void SetForwardAndTurn(Vector3 movement)
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
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }
        }
    }
}