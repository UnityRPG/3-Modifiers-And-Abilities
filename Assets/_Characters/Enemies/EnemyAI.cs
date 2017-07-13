﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float deathVanishSeconds = 2.0f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] [Range(0f, 20f)] float minWaitTime = 0f;
        [SerializeField] [Range(0f, 60f)] float maxWaitTime = 2.0f;

		float lastHitTime = 0f;
        WeaponSystem weaponSystem;
        PlayerControl player;
        Character character = null;

        void Start()
        {
			weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();

            if (patrolPath != null)
            {
                StartCoroutine(Patrol(patrolPath));
            }
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

			float weaponHitPeriod = weaponSystem.GetCurrentWeapon().GetMinTimeBetweenHits();
            float timeToWait = weaponHitPeriod * character.GetAnimSpeedMultiplier();
            bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

            if (distanceToPlayer < currentWeaponRange && isTimeToHitAgain)
            {
                weaponSystem.AttackTarget(player.gameObject);
                lastHitTime = Time.time;
            }
        }

        public float GetDeathVanishDelay()
        {
            return deathVanishSeconds;
        }

	    void OnDrawGizmos()
        {
            // Draw attack sphere 
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }


        IEnumerator Patrol(PatrolPath path)
        {
            int childIndex = 0;
            while (true)
            {
                character.SetDestination(path.transform.GetChild(childIndex).position);
                if (Vector3.Distance(transform.position, path.transform.GetChild(childIndex).transform.position) <= waypointTolerance)
                {
                    childIndex = (childIndex + 1) % path.transform.childCount;
                }
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            }
        }
	}
}