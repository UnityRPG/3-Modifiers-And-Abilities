using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {
        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
		[SerializeField] float weaponDamageBonus = 5f;
        [SerializeField] float maxAttackRange = 2.0f;
        [SerializeField] bool isRanged = false;
        [SerializeField] float hitOrFireTime = 0.5f;

        public float GetMinTimeBetweenHits()
        {
            return attackAnimation.length;
        }

        public float GetAnimHitTime()
        {
            // consider routing from animator event for more precision
            return hitOrFireTime;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }
        
        public AnimationClip GetAttackAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

		public float GetWeaponDamageBonus()
		{
			return weaponDamageBonus;
		}

		public bool IsRanged()
		{
			return isRanged;
		}

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}