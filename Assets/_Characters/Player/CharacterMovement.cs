using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI; // TODO consider re-wiring

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))] // consider removing too?
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1f;

        ThirdPersonCharacter character = null;   // A reference to the ThirdPersonCharacter on the object
        NavMeshAgent agent = null;

        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
			character = GetComponent<ThirdPersonCharacter>();

            agent = GetComponentInChildren<NavMeshAgent>();
			agent.updateRotation = false;
			agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;

            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        // todo move to ThirdPersonCharacter then rename CharacterMovement
        void Update()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity);
            }
            else
            {
                character.Move(Vector3.zero);
            }
        }

        public void SetDestination(Vector3 worldPos)
        {
            agent.destination = worldPos;
        }

		// todo move to player movement
		void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                agent.SetDestination(enemy.transform.position);
            }
        }
    }
}