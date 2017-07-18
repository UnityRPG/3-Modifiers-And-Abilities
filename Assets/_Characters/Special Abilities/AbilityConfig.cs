using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Spcial Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AnimationClip abilityAnimation = null;
        [SerializeField] AudioClip[] abilitySounds = null;

        protected AbilityBehaviour behaviour; // accessible to the config sub-classes

        // abstract so that we can specifiy the concrete type in the sub-class
        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToAttachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToAttachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AnimationClip GetAbilityAnimation()
        {
            return abilityAnimation;
        }

        public AudioClip GetRandomAbilitySound()
        {
            return abilitySounds[Random.Range(0, abilitySounds.Length)];
        }
    }
}