﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility {

    AreaEffectConfig config;
	AudioSource audioSource = null;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetConfig(AreaEffectConfig configToSet)
    {
        this.config = configToSet;
    }

    public void Use(AbilityUseParams useParams)
    {
        DealRadialDamage(useParams);
        PlayParticleEffect();
		audioSource.clip = config.GetAudioClip();
		audioSource.Play();
    }

    private void PlayParticleEffect()
    {
        var particlePrefab = config.GetParticlePrefab();
        var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
        // TODO decide if particle system attaches to player
        ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
        myParticleSystem.Play();
        Destroy(prefab, myParticleSystem.main.duration);
    }

    private void DealRadialDamage(AbilityUseParams useParams)
    {
        // Static sphere cast for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            config.GetRadius(),
            Vector3.up,
            config.GetRadius()
        );

        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget(); // TODO ok Rick?
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
