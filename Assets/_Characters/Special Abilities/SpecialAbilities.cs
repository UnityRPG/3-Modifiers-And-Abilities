using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
		[SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar = null;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;

        float currentEnergyPoints;
        CameraUI.CameraRaycaster cameraRaycaster;
        Weapons weapon;

        // Use this for initialization
        void Start()
        {
            weapon = GetComponentInParent<Weapons>();
            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

		void AttachInitialAbilities()
		{
			for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
			{
				abilities[abilityIndex].AttachAbilityTo(gameObject);
			}
		}

        public void AttemptSpecialAbility(int abilityIndex, EnemyAI target = null)
		{
            print("Attempting ability " + abilityIndex); 
			var energyComponent = GetComponent<SpecialAbilities>();
			var energyCost = abilities[abilityIndex].GetEnergyCost();

			if (energyComponent.IsEnergyAvailable(energyCost))
			{
				energyComponent.ConsumeEnergy(energyCost);
				var abilityParams = new AbilityUseParams(target, weapon.GetTotalDamagePerHit());
				abilities[abilityIndex].Use(abilityParams);
			}
		}

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        void UpdateEnergyBar()
        {
            if (energyBar) // Enemies may not have energy bars to update
            {
                energyBar.fillAmount = EnergyAsPercent();
            }
        }

        float EnergyAsPercent()
        {
            return currentEnergyPoints / maxEnergyPoints;
        }
    }
}