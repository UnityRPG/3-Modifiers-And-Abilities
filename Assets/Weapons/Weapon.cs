using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("RPG/Weapon"))]
public class Weapon : ScriptableObject {

    public Transform gripTransform;

    [SerializeField] GameObject weaponPrefab; 
    [SerializeField] AnimationClip attackAnimation;

    public GameObject GetWeaponPrefab() {
        return weaponPrefab;
    }
}
