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
        [SerializeField] float hitOrFireTime = 0.5f;

        public float GetMinTimeBetweenHits()
        {
            return attackAnimation.length;
        }

        public float GetAnimHitTime()
        {
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

        // So that asset packs cannot cause crashes
        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}