using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;
using System;

public class AreaEffectBehaviour : AbilityBehaviour {



	// Use this for initialization
	void Start () {
		print("Area Effect behaviour attached to " + gameObject.name);
	}

    public override void Use(AbilityUseParams useParams)
    {
        DealRadialDamage(useParams);
        PlayParticleInWorldSpace();
    }

    private void DealRadialDamage(AbilityUseParams useParams)
    {
        print("Area Effect used by " + gameObject.name);
        // Static sphere cast for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            (config as AreaEffectConfig).GetRadius(),
            Vector3.up,
            (config as AreaEffectConfig).GetRadius()
        );

        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damageToDeal = useParams.baseDamage + (config as AreaEffectConfig).GetDamageToEachTarget(); // TODO ok Rick?
                damageable.AdjustHealth(damageToDeal);
            }
        }
    }
}
