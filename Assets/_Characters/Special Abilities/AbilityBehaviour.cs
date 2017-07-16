using System.Collections;
﻿using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;

        const string ATTACK_TRIGGER = "Attack";
		const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";
        const float PARTICLE_CLEAN_UP_DELAY = 20f;

        public abstract void Use(GameObject target = null);

        public void SetConfig(AbilityConfig configToSet)
		{
			config = configToSet;
		}

		protected void PlayParticleEffect()
		{
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(
                particlePrefab,
                transform.position,
                particlePrefab.transform.rotation
            );
            particleObject.transform.parent = transform; // set world space in preab if required
            particleObject.GetComponent<ParticleSystem>().Play();
			StartCoroutine(DestroyParticleWhenFinished(particleObject));
		}

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return null;
        }

        protected void PlayAbilityAnimation()
        {
            var animatorOverrideController = GetComponent<Character>().GetOverrideController();
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK_STATE] = config.GetAbilityAnimation();
            animator.SetTrigger(ATTACK_TRIGGER);
		}

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
			var audioSource = GetComponent<AudioSource>();
            audioSource.clip = abilitySound;
			audioSource.Play();
        }
    }
}