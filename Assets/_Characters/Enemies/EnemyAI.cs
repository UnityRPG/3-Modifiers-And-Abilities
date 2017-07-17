using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] [Range(0f, 20f)] float minWaitTime = 0f;
        [SerializeField] [Range(0f, 60f)] float maxWaitTime = 2.0f;

        float lastHitTime;
        WeaponSystem weaponSystem;
        PlayerControl player;
        Character character;
        int nextWaypoint;
        float currentWeaponRange;
        float distanceToPlayer;

        enum State { idle, patrolling, attacking, chasing }
        State state = State.idle;

        void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();
        }

        void Update()
        {

            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                StartCoroutine(AttackPlayer());
            }
        }

        IEnumerator Patrol()
        {
            state = State.patrolling;
            while (patrolPath != null) // patrol forever, or at least until childIndex overflows!
            {
                character.SetDestination(patrolPath.transform.GetChild(nextWaypoint).position);
                if (Vector3.Distance(transform.position, patrolPath.transform.GetChild(nextWaypoint).transform.position) <= waypointTolerance)
                {
                    nextWaypoint = (nextWaypoint + 1) % patrolPath.transform.childCount;
                }
                yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator AttackPlayer()
        {
            state = State.attacking;
            while (true)
            {
                float weaponHitPeriod = weaponSystem.GetCurrentWeapon().GetMinTimeBetweenHits();
                float timeToWait = weaponHitPeriod * character.GetAnimSpeedMultiplier();
                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;
                if (isTimeToHitAgain)
                {
                    weaponSystem.AttackTarget(player.gameObject);
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        void OnDrawGizmos()
        {
            // Draw chase sphere 
            Gizmos.color = new Color(255, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw chase sphere 
            Gizmos.color = new Color(0, 0, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}