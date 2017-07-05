﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;
		AudioSource audioSource = null;

        public void SetConfig(PowerAttackConfig configToSet)
        {
            this.config = configToSet;
        }

        // Use this for initialization
        void Start()
        {
            print("Power Attack behaviour attached to " + gameObject.name);
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Use(AbilityUseParams useParams)
        {
            print("Power attack used by: " + gameObject.name);
            DealDamage(useParams);
            PlayParticleEffect(); // TODO find way of moving audio to parent class
			audioSource.clip = config.GetAudioClip();
			audioSource.Play();
        }

		private void PlayParticleEffect()
		{
			var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
			// TODO decide if particle system attaches to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
			myParticleSystem.Play();
			Destroy(prefab, myParticleSystem.main.duration);
		}

        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}