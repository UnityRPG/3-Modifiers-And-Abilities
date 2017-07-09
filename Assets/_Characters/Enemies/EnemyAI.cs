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

		// todo push these down from here and Player to Character
		const string ATTACK_TRIGGER = "Attack";
		float lastHitTime = 0f;
        bool isAttacking = false;
        Animator animator;
        WeaponSystem mainWeapon;
        PlayerControl player;

        void Start()
        {
            animator = GetComponent<Animator>();
			mainWeapon = GetComponent<WeaponSystem>();
            player = FindObjectOfType<PlayerControl>();
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, player.transform.position) < mainWeapon.GetCurrentWeapon().GetMaxAttackRange())
            {
                AttackPlayer();   
            }
        }

        private void AttackPlayer()
		{
			if (Time.time - lastHitTime > mainWeapon.GetCurrentWeapon().GetMinTimeBetweenHits())
			{
				animator.SetTrigger(ATTACK_TRIGGER);
				HealthSystem enemyDamageSystem = player.GetComponent<HealthSystem>();
				enemyDamageSystem.AdjustHealth(mainWeapon.GetTotalDamagePerHit());
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