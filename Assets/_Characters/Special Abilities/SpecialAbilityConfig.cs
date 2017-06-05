using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class SpecialAbilityConfig : ScriptableObject
    {
        [Header("Spcial Ability General")]
        [SerializeField] float energyCost = 10f;

        protected ISpecialAbility behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToattachTo);

        public void Use()
        {
            behaviour.Use();
        }
    }
}