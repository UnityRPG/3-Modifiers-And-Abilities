using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            DealDamage(target);
            PlayParticleInWorldSpace();
        }

        private void DealDamage(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).GetDamage();
            var damageableTarget = target.GetComponent<HealthSystem>();
            if (damageableTarget)
            {
                PlayAbilityAnimation();
                damageableTarget.AdjustHealth(damageToDeal);
            }
        }
    }
}