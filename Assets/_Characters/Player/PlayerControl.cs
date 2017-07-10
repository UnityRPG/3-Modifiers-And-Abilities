using UnityEngine;

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
        WeaponSystem weaponSystem;
        HealthSystem damageSystem;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<CharacterMovement>();
            abilities = GetComponent<SpecialAbilities>();
            animator = GetComponent<Animator>();
            damageSystem = GetComponent<HealthSystem>();
            weaponSystem = GetComponent<WeaponSystem>();

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
            if (Time.time - lastHitTime > weaponSystem.GetCurrentWeapon().GetMinTimeBetweenHits())
            {
                animator.SetTrigger(ATTACK_TRIGGER);
                HealthSystem enemyDamageSystem = enemy.GetComponent<HealthSystem>();
                enemyDamageSystem.AdjustHealth(weaponSystem.GetTotalDamagePerHit());
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}