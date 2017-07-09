﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO consider re-wire
using RPG.Core;

namespace RPG.Characters
{
    public class EnemyAI : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100f;

        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float attackRadius = 4f;

        [SerializeField] float firingPeriodInS = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;

        bool isAttacking = false;
        float currentHealthPoints;
        PlayerControl player = null;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public void AdjustHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }

        void Start()
        {
            player = FindObjectOfType<PlayerControl>();
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            if (player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                Destroy(this); // To stop enemy behaviour
            }

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelay = Random.Range(firingPeriodInS - firingPeriodVariation, firingPeriodInS + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay);
            }

            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius)
            {
                // aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                //aiCharacterControl.SetTarget(transform);
            }
        }   

        void OnDrawGizmos()
        {
            // Draw attack sphere 
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}