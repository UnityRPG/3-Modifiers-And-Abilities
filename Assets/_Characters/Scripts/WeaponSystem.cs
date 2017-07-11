using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] WeaponConfig startingWeapon = null;
        [SerializeField] float characterBaseDamage = 10f;
        [SerializeField] float weaponDamageBonus = 5f;

        WeaponConfig currentWeaponConfig;
        GameObject weaponObject;
        Animator animator;

		const string ATTACK_TRIGGER = "Attack";

        // Use this for initialization
        void Start()
        {
			animator = GetComponent<Animator>();

            currentWeaponConfig = startingWeapon;
            PutWeaponInHand(currentWeaponConfig);
            SetupRuntimeAnimator();
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return startingWeapon;
        }

        public float GetTotalDamagePerHit()
        {
            return characterBaseDamage + weaponDamageBonus;
        }


		public void AttackTarget(GameObject target)
		{
			transform.LookAt(target.transform);
			animator.SetTrigger(ATTACK_TRIGGER);
			float hitTime = GetCurrentWeapon().GetAnimHitTime();
			StartCoroutine(DamageTargetAfterSeconds(target, hitTime));
		}

		IEnumerator DamageTargetAfterSeconds(GameObject target, float seconds)
		{
			yield return new WaitForSecondsRealtime(seconds);
			HealthSystem enemyDamageSystem = target.GetComponent<HealthSystem>();
			enemyDamageSystem.AdjustHealth(GetTotalDamagePerHit());
		}

		public void PutWeaponInHand(WeaponConfig weaponToUse)
		{
			this.startingWeapon = weaponToUse;
			var weaponPrefab = startingWeapon.GetWeaponPrefab();
			GameObject dominantHand = RequestDominantHand();
			Destroy(weaponObject);
			weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
			weaponObject.transform.localPosition = startingWeapon.gripTransform.localPosition;
			weaponObject.transform.localRotation = startingWeapon.gripTransform.localRotation;
		}

        private void SetupRuntimeAnimator()
        {
            if (!animatorOverrideController)
            {
                Debug.LogAssertion("Please provide " + gameObject + " with an animator override controller.");
            }
            else
            {
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController["DEFAULT ATTACK"] = startingWeapon.GetAttackAnimClip(); // remove const
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