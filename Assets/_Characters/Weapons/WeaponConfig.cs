using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] Transform gripTransform;
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float weaponDamageBonus = 5f;
        [SerializeField] float maxAttackRange = 2.0f;
        [SerializeField] float damageDelay = 0.5f;

        public Quaternion GetGripRotation()
        {
            return gripTransform.localRotation;
        }

        public Vector3 GetGripPosition()
        {
            return gripTransform.localPosition;
        }

        public float GetMinTimeBetweenHits()
        {
            return attackAnimation.length;
        }

        public float GetAnimHitTime()
        {
            return damageDelay;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public float GetWeaponDamageBonus()
        {
            return weaponDamageBonus;
        }

        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        // So that asset packs cannot cause crashes
        void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}