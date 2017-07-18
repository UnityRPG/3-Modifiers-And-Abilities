using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    [SelectionBase]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        AudioSource audioSource;

        void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();

            DestroyChildren();
            InstantiateWeapon();
        }

        void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }


        void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        void OnTriggerEnter()
        {
            FindObjectOfType<PlayerControl>().GetComponent<WeaponSystem>().PutWeaponInHand(weaponConfig);
            audioSource.PlayOneShot(pickUpSFX);
        }
    }
}