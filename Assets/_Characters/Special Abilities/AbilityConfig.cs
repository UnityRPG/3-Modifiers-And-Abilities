﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Spcial Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;

        protected AbilityBehaviour behaviour;

        public void AttachAbilityToPlayer<T>()
        {
            var player = GameObject.FindWithTag("Player");
            var behviourComponent = player.AddComponent<AreaEffectBehaviour>();
            behviourComponent.SetConfig(this); // ok or need to be child?
            behaviour = behviourComponent;
        }

        public void Use(AbilityUseParams useParams)
        {
            behaviour.Use(useParams);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }
    }
}