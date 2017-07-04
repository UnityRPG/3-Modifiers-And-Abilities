using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;

        public abstract void Use(AbilityUseParams useParams);

        public void SetConfig(AbilityConfig configToSet)
		{
			this.config = configToSet;
		}

		protected void PlayParticleInWorldSpace()
        {
            SetupParticleEffect();
        }

		protected void PlayParticleInPlayerSpace()
		{
            SetupParticleEffect();
            // Attach to player
		}

        private void SetupParticleEffect()
        {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}