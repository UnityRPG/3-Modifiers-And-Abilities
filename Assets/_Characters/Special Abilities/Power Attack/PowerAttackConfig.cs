using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Power Attack"))]
    public class PowerAttackConfig : AbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField] float damage = 10f;

		public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
		{
            return objectToAttachTo.AddComponent<PowerAttackBehaviour>();
		}

        public float GetDamage()
        {
            return damage;
        }
    }
}