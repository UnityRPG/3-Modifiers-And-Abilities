using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Self Heal"))]
    public class SelfHealConfig : SpecialAbility
	{
		[Header("Self Heal Specific")]
		[SerializeField] float extraHealth = 50f;

		public override void AttachComponentTo(GameObject gameObjectToattachTo)
		{
			var behviourComponent = gameObjectToattachTo.AddComponent<SelfHealBehaviour>();
			behviourComponent.SetConfig(this);
			behaviour = behviourComponent;
		}

		public float GetExtraHealth()
		{
			return extraHealth;
		}
	}
}