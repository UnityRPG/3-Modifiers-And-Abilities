using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {
        SelfHealConfig config = null;
        Player player = null;
        AudioSource audioSource = null;

        void Start()
        {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig(SelfHealConfig configToSet)
		{
			this.config = configToSet;
		}

		public void Use(AbilityUseParams useParams)
		{
            player.Heal(config.GetExtraHealth());
            audioSource.clip = config.GetAudioClip(); // TODO find way of moving audio to parent class
            audioSource.Play();
            PlayParticleEffect();
		}

		private void PlayParticleEffect()
		{
			var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            prefab.transform.parent = transform;
			ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
			myParticleSystem.Play();
			Destroy(prefab, myParticleSystem.main.duration);
		}
    }
}