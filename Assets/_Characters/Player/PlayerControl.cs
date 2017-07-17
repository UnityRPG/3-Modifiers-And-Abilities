﻿using System.Collections;
using UnityEngine;

using RPG.CameraUI; // for mouse events

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(SpecialAbilities))]
    public class PlayerControl : MonoBehaviour
    {
        float lastHitTime = 0f;
        Character character = null;
        SpecialAbilities abilities = null;
        WeaponSystem weaponSystem;

        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();

            RegisterForMouseEvents();
        }

        void Update()
        {
            ScanForAbilityKeyDown();
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
            var cameraRaycaster = FindObjectOfType<CameraUI.CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(EnemyAI enemy) // note co-routine
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(RepeatAttack(enemy));
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject))
            {
                PowerAttack(enemy);
            }
            else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject))
            {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject)); // Execute in series
            yield return StartCoroutine(RepeatAttack(enemy));
        }

		IEnumerator MoveAndPowerAttack(EnemyAI enemy)
		{
			yield return StartCoroutine(MoveToTarget(enemy.gameObject)); // Execute in series
            PowerAttack(enemy);
		}

        void PowerAttack(EnemyAI enemy)
        {
			abilities.AttemptSpecialAbility(0, enemy.gameObject);
        }

        IEnumerator RepeatAttack(EnemyAI enemy)
        {
            var enemyHealth = enemy.GetComponent<HealthSystem>();

			float weaponHitPeriod = weaponSystem.GetCurrentWeapon().GetMinTimeBetweenHits();
			bool timeToHitAgain = Time.time - lastHitTime > weaponHitPeriod;

            while (enemyHealth.healthAsPercentage > 0 && timeToHitAgain)
            {
                weaponSystem.AttackTarget(enemy.gameObject);
                lastHitTime = Time.time;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            while (!IsTargetInRange(target))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}