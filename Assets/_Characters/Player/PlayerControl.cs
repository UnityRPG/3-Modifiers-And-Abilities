using UnityEngine;

using RPG.CameraUI; // for mouse events

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
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
            float weaponHitPeriod = weaponSystem.GetCurrentWeapon().GetMinTimeBetweenHits();
            bool timeToHitAgain = Time.time - lastHitTime > weaponHitPeriod;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject) && timeToHitAgain)
            {
                characterMovement.AttackTarget(enemy.gameObject);
                lastHitTime = Time.time;
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

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }
    }
}