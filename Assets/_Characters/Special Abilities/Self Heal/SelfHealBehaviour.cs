using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        DamageSystem damageSystem;

        void Start()
        {
            damageSystem = GetComponent<DamageSystem>();
        }

		public override void Use(AbilityUseParams useParams)
		{
			print("Self heal used by: " + gameObject.name);
            PlayParticleInPlayerSpace();
            damageSystem.AdjustHealth(-(config as SelfHealConfig).GetExtraHealth()); // note -ve
		}
    }
}