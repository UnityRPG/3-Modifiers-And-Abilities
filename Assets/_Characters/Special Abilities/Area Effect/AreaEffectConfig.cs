﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Area Effect"))]
    public class AreaEffectConfig : SpecialAbility
    {
        [Header("Area Effect Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damageToEachTarget = 15f;

        public override void AttachComponentTo(GameObject gameObjectToattachTo)
        {
            var behviourComponent = gameObjectToattachTo.AddComponent<AreaEffectBehaviour>();
            behviourComponent.SetConfig(this);
            behaviour = behviourComponent;
        }

        public float GetDamageToEachTarget()
        {
            return damageToEachTarget;
        }

        public float GetRadius()
        {
            return radius;
        }
    }
}