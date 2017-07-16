﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    public class SpecialAbilities : MonoBehaviour
    {
		[SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;

        float currentEnergyPoints;

		public float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

         
        void Start()
        {
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

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
		{
			var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints)
			{
				ConsumeEnergy(energyCost);
				abilities[abilityIndex].Use(target);
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
                energyBar.fillAmount = energyAsPercent;
            }
        }
    }
}