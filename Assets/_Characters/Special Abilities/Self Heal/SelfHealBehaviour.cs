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
			print("Self heal used by: " + gameObject.name);
            PlayParticleInPlayerSpace();
            healthSystem.AdjustHealth(-(config as SelfHealConfig).GetExtraHealth()); // note -ve
		}
    }
}