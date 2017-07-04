using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {
        SelfHealConfig config;
        Player player;

        void Start()
        {
            player = GetComponent<Player>();
        }

        public void SetConfig(SelfHealConfig configToSet)
		{
			this.config = configToSet;
		}

		public void Use(AbilityUseParams useParams)
		{
			print("Self heal used by: " + gameObject.name);
            player.AdjustHealth(-config.GetExtraHealth()); // note -ve
		}
    }
}