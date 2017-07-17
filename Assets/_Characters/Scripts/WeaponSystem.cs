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

            PutWeaponInHand(startingWeapon);
            SetupRuntimeAnimator();
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public float GetTotalDamagePerHit()
        {

            return characterBaseDamage + currentWeaponConfig.GetWeaponDamageBonus();
        }

        public void AttackTarget(GameObject target)
        {
            transform.LookAt(target.transform);
            animator.SetTrigger(ATTACK_TRIGGER);
            float hitTime = GetCurrentWeapon().GetAnimHitTime();
            StartCoroutine(DamageAndWait(target, hitTime));
        }

        IEnumerator DamageAndWait(GameObject target, float seconds)
        {
            HealthSystem enemyDamageSystem = target.GetComponent<HealthSystem>();
            enemyDamageSystem.TakeDamage(GetTotalDamagePerHit());
			yield return new WaitForSecondsRealtime(seconds);
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

        private void SetupRuntimeAnimator()
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

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on " + gameObject.name + " , please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on " + gameObject.name + " , please remove one");
            return dominantHands[0].gameObject;
        }
    }
}