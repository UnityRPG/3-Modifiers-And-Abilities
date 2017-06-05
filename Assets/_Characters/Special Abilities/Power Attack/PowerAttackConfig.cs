﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField]
        float extraDamage = 10f;

        public override void AttachComponentTo(GameObject gameObjectToattachTo)
        {
            var behviourComponent = gameObjectToattachTo.AddComponent<PowerAttackBehaviour>();
            behviourComponent.SetConfig(this);
            behaviour = behviourComponent;
        }
    }
}