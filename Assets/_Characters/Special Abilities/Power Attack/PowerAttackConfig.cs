﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Power Attack"))]
    public class PowerAttackConfig : AbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float extraDamage = 10f;

		public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
		{
            return objectToAttachTo.AddComponent<PowerAttackBehaviour>();
		}

        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }
}