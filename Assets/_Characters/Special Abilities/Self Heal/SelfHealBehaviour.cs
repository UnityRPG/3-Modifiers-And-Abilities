using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerControl player;

        void Start()
        {
            player = GetComponent<PlayerControl>();
        }

		public override void Use(AbilityUseParams useParams)
		{
			print("Self heal used by: " + gameObject.name);
            PlayParticleInPlayerSpace();
            player.AdjustHealth(-(config as SelfHealConfig).GetExtraHealth()); // note -ve
		}
    }
}