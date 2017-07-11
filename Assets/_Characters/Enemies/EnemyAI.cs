﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [SelectionBase]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float deathVanishSeconds = 2f;

		float lastHitTime = 0f;
        WeaponSystem weaponSystem;
        PlayerControl player;
        CharacterMovement characterControl = null;

        void Start()
        {
			weaponSystem = GetComponent<WeaponSystem>();
            characterControl = GetComponent<CharacterMovement>();
            player = FindObjectOfType<PlayerControl>();
        }

        public float GetDeathVanishDelay()
        {
            return deathVanishSeconds;
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

			float weaponHitPeriod = weaponSystem.GetCurrentWeapon().GetMinTimeBetweenHits();
            float timeToWait = weaponHitPeriod * characterControl.GetAnimSpeedMultiplier();
            bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

            if (distanceToPlayer < currentWeaponRange && isTimeToHitAgain)
            {
                weaponSystem.AttackTarget(player.gameObject);
                lastHitTime = Time.time;
            }
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
    }
}