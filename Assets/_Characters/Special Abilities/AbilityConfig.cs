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
        const string NAMESPACE = "RPG.Characters.";

        public void AttachAbilityToPlayer(string typeName)
        {
            AbilityBehaviour behaviourComponent = null;

            var player = GameObject.FindWithTag("Player");
            if (typeName == NAMESPACE + "AreaEffectConfig")
            {
                behaviourComponent = player.AddComponent<AreaEffectBehaviour>();
            }
            else if (typeName == NAMESPACE + "SelfHealConfig")
            {
                behaviourComponent = player.AddComponent<SelfHealBehaviour>();
            }
            else if (typeName == NAMESPACE + "PowerAttackConfig")
            {
                behaviourComponent = player.AddComponent<PowerAttackBehaviour>();
            }
            else
            {
                Debug.LogAssertion("Ability type not known");
                return;
            }
            behaviourComponent.SetConfig(this); // ok or need to be child?
            behaviour = behaviourComponent;
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