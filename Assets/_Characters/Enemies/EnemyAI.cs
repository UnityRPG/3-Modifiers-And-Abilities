﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f; // todo impliment?
        [SerializeField] float deathVanishSeconds = 2.0f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] [Range(0f, 20f)] float minWaitTime = 0f;
        [SerializeField] [Range(0f, 60f)] float maxWaitTime = 2.0f;

		float lastHitTime = 0f;
        WeaponSystem weaponSystem;
        PlayerControl player;
        Character character = null;
        int nextWaypoint = 0;

        enum State  {idle, patrolling, attacking, chasing }
        State state = State.idle;

        void Start()
        {
			weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();
            AttemptToPatrol();
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

			float weaponHitPeriod = weaponSystem.GetCurrentWeapon().GetMinTimeBetweenHits();
            float timeToWait = weaponHitPeriod * character.GetAnimSpeedMultiplier();
            bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

			if (distanceToPlayer > currentWeaponRange)
			{
				AttemptToPatrol();
			}

            if (distanceToPlayer < currentWeaponRange)
            {
                StopAllCoroutines(); // consider getting handle and being specific
                state = State.attacking;
            }

            if (isTimeToHitAgain && state == State.attacking)
            {
                StopAllCoroutines();
                weaponSystem.AttackTarget(player.gameObject);
                lastHitTime = Time.time;
            }
        }

        void AttemptToPatrol()
		{
            if (patrolPath != null && state != State.patrolling)
			{
				state = State.patrolling;
				StartCoroutine(Patrol(patrolPath));
			}  
        }

        public float GetDeathVanishDelay()
        {
            return deathVanishSeconds;
        }

	    void OnDrawGizmos()
        {
            // todo show current weapon range

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }


        IEnumerator Patrol(WaypointContainer path)
        {
            while (true) // patrol forever, or at least until childIndex overflows!
            {
                character.SetDestination(path.transform.GetChild(nextWaypoint).position);
                if (Vector3.Distance(transform.position, path.transform.GetChild(nextWaypoint).transform.position) <= waypointTolerance)
                {
                    nextWaypoint = (nextWaypoint + 1) % path.transform.childCount;
                }
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            }
        }
	}
}