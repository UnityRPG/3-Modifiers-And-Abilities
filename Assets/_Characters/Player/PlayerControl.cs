﻿﻿﻿﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

using RPG.CameraUI; // for mouse events

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
        const string ATTACK_TRIGGER = "Attack";

        AudioSource audioSource;
        Animator animator;
        float currentHealthPoints;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;
        CharacterMovement characterMovement = null;
        SpecialAbilities abilities = null;
        MainWeapon mainWeapon;
        HealthSystem damageSystem;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<CharacterMovement>();
            abilities = GetComponent<SpecialAbilities>();
            animator = GetComponent<Animator>();
            damageSystem = GetComponent<HealthSystem>();
            // todo set main weapon?

            RegisterForMouseEvents();
        }

		void Update()
		{
			if (damageSystem.healthAsPercentage > Mathf.Epsilon)
			{
				ScanForAbilityKeyDown();
			}
		}

		private void ScanForAbilityKeyDown()
		{
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
			{
				if (Input.GetKeyDown(keyIndex.ToString()))
				{
                    abilities.AttemptSpecialAbility(keyIndex);
				}
			}
		}

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraUI.CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
			cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

		void OnMouseOverPotentiallyWalkable(Vector3 destination)
		{
			if (Input.GetMouseButton(0))
			{
                characterMovement.SetDestination(destination);
			}
		}

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                AttackTarget(enemy);
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy.gameObject))
            {
                characterMovement.SetDestination(enemy.transform.position);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0, enemy.GetComponent<HealthSystem>()); // todo consdier moving to start
            }
        }

        private void AttackTarget(EnemyAI enemy)
        {
            if (Time.time - lastHitTime > mainWeapon.GetCurrentWeapon().GetMinTimeBetweenHits())
            {
                animator.SetTrigger(ATTACK_TRIGGER);
                HealthSystem enemyDamageSystem = enemy.GetComponent<HealthSystem>();
                enemyDamageSystem.AdjustHealth(mainWeapon.GetTotalDamagePerHit());
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= mainWeapon.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}