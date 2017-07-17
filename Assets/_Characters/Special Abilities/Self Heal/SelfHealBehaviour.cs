using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        HealthSystem healthSystem;

        void Start()
        {
            healthSystem = GetComponent<HealthSystem>();
        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            PlayParticleEffect();
            healthSystem.Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}