using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using RPG.Characters;
using RPG.Core;

namespace RPG.Characters
{
    public class Weapons : MonoBehaviour
    {

        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Weapon weaponConfig = null;

        GameObject weaponObject;
        Animator animator;

        // Use this for initialization
        void Start()
        {
            PutWeaponInHand(weaponConfig);
            SetupRuntimeAnimator();
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            this.weaponConfig = weaponToUse;
            var weaponPrefab = weaponConfig.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = weaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = weaponConfig.gripTransform.localRotation;
        }

        public Weapon GetCurrentWeapon()
        {
            return weaponConfig;
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponConfig.GetAttackAnimClip(); // remove const
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on " + gameObject.name + " , please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on " + gameObject.name + " , please remove one");
            return dominantHands[0].gameObject;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}