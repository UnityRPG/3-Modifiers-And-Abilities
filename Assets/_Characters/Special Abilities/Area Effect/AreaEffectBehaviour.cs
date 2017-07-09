using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaEffectBehaviour : AbilityBehaviour {

    public override void Use(AbilityUseParams useParams)
    {
        DealRadialDamage(useParams);
        PlayParticleInWorldSpace();
    }

    private void DealRadialDamage(AbilityUseParams useParams)
    {
        // Static sphere cast for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            (config as AreaEffectConfig).GetRadius(),
            Vector3.up,
            (config as AreaEffectConfig).GetRadius()
        );

        foreach (RaycastHit hit in hits)
        {
            var gameObjectHit = hit.collider.gameObject;
            var damageable = gameObjectHit.GetComponent<IDamageable>();

            if (damageable != null  && gameObjectHit.tag != "Player")
            {
                float damageToDeal = useParams.baseDamage + (config as AreaEffectConfig).GetDamageToEachTarget();
                damageable.AdjustHealth(damageToDeal);
            }
        }
    }
}
