using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField]
        float extraDamage = 10f;

        public override ISpecialAbility AddComponent(GameObject gameObjectToattachTo)
        {
            var behviourComponent = gameObjectToattachTo.AddComponent<PowerAttackBehaviour>();
            behviourComponent.SetConfig(this);
            return behviourComponent;
        }
    }
}