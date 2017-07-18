using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] WeaponConfig startingWeapon;
        [SerializeField] float characterBaseDamage = 10f;

        float lastHitTime;
        WeaponConfig currentWeaponConfig;
        GameObject weaponObject;
        Animator animator;
        Character character;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";

        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
            SetupRuntimeAnimator();

            PutWeaponInHand(startingWeapon);
        }

        void Update()
        {
            var characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            if (characterHealth <= Mathf.Epsilon)
            {
                StopAllCoroutines();
            }
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public float GetTotalDamagePerHit()
        {

            return characterBaseDamage + currentWeaponConfig.GetWeaponDamageBonus();
        }

        public void RepeatAttack(GameObject target)
        {
            StartCoroutine(AttackEverySoOften(target));
        }

        IEnumerator AttackEverySoOften(GameObject target)
        {
            bool stillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            while (stillAlive)
            {
                float weaponHitPeriod = currentWeaponConfig.GetMinTimeBetweenHits();
                float timeToWait = weaponHitPeriod * character.GetAnimSpeedMultiplier();
                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;
                if (isTimeToHitAgain)
                {
                    AttackTarget(target);
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        void AttackTarget(GameObject target)
        {
            {
                transform.LookAt(target.transform);
                animator.SetTrigger(ATTACK_TRIGGER);
                float damageDelay = GetCurrentWeapon().GetAnimHitTime();
                StartCoroutine(DamageAfterDelay(target, damageDelay));
            }
        }

        IEnumerator DamageAfterDelay(GameObject target, float delay)
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
                animatorOverrideController[DEFAULT_ATTACK_STATE] = startingWeapon.GetAttackAnimClip();
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