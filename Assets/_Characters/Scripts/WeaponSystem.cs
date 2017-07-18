using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] WeaponConfig currentWeaponConfig;
        [SerializeField] float characterBaseDamage = 10f;

        float lastHitTime;
        GameObject weaponObject;
        Animator animator;
        Character character;
        GameObject target;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
            SetupRuntimeAnimator();

            PutWeaponInHand(currentWeaponConfig);
        }

        void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;
            if (target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else 
            {
                var targetHealth = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targetHealth <= Mathf.Epsilon;

                var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
            }

            var characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = characterHealth <= Mathf.Epsilon;
            if (characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }
        }

        public void StopAttacking() // so that can be stopped from other classes
        {
            StopAllCoroutines();
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public float GetTotalDamagePerHit()
        {

            return characterBaseDamage + currentWeaponConfig.GetWeaponDamageBonus();
        }

        public void AttackTarget(GameObject targetToAtToAttack)
        {
            
            target = targetToAtToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }


        IEnumerator AttackTargetRepeatedly()
        {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            while (attackerStillAlive && targetStillAlive)
            {
                float weaponHitPeriod = currentWeaponConfig.GetMinTimeBetweenHits();
                float timeToWait = weaponHitPeriod * character.GetAnimSpeedMultiplier();
                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;
                if (isTimeToHitAgain)
                {
                    AttackTarget();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        void AttackTarget()
        {
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float damageDelay = GetCurrentWeapon().GetAnimHitTime();
            SetupRuntimeAnimator();
            StartCoroutine(DamageAfterDelay(damageDelay));
        }

        IEnumerator DamageAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            target.GetComponent<HealthSystem>().TakeDamage(GetTotalDamagePerHit());
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.GetGripPosition();
            weaponObject.transform.localRotation = currentWeaponConfig.GetGripRotation();
        }

        void SetupRuntimeAnimator()
        {
            if (!character.GetOverrideController())
            {
                Debug.Break(); // we don't want to carry-on until fixed
                Debug.LogAssertion("Please provide " + gameObject + " with an animator override controller.");
            }
            else
            {
                var animatorOverrideController = character.GetOverrideController();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController[DEFAULT_ATTACK_STATE] = currentWeaponConfig.GetAttackAnimClip();
            }
        }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on " + gameObject.name + " , please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on " + gameObject.name + " , please remove one");
            return dominantHands[0].gameObject;
        }
    }
}