using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        HealthSystem damageSystem;

        void Start()
        {
            damageSystem = GetComponent<HealthSystem>();
        }

		public override void Use(AbilityUseParams useParams)
		{
			print("Self heal used by: " + gameObject.name);
            PlayParticleInPlayerSpace();
            damageSystem.AdjustHealth(-(config as SelfHealConfig).GetExtraHealth()); // note -ve
		}
    }
}