﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using RPG.Characters; // to identify player for scene reload

[RequireComponent(typeof(Character))]
public class HealthSystem : MonoBehaviour{

	[SerializeField] float maxHealthPoints = 100f;
	[SerializeField] Image healthBar = null;
	[SerializeField] AudioClip[] damageSounds;
	[SerializeField] AudioClip[] deathSounds;
	[SerializeField] float deathVanishSeconds = 2.0f;

	const string DEATH_TRIGGER = "Death";

    bool isInDeathThrows = false;
	float currentHealthPoints;
    Animator animator;
    AudioSource audioSource;

	public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

	 
	void Start () {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealthPoints = maxHealthPoints;
	}
	
	
	void Update () {
        UpdateHealthBar();
	}

	void UpdateHealthBar()
	{
        if (healthBar) // Enemies may not have energy bars to update
		{
            healthBar.fillAmount = healthAsPercentage;
		}
	}

    public void TakeDamage(float damage)
	{
        if (isInDeathThrows) { return; }

        bool charaterDies = (currentHealthPoints - damage <= 0); // must ask before reducing health
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
		audioSource.clip = damageSounds[Random.Range(0, damageSounds.Length)];
		audioSource.Play();
		if (charaterDies)
		{
			StartCoroutine(KillCharacter());
		}
	}

    public void Heal(float amount)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints + amount, 0f, maxHealthPoints);
    }

    IEnumerator KillCharacter()
    {
		isInDeathThrows = true;
        animator.SetTrigger(DEATH_TRIGGER);
        var playerComponent = GetComponent<PlayerControl>();
        if (playerComponent && playerComponent.isActiveAndEnabled)
        {
			audioSource.clip = deathSounds[Random.Range(0, deathSounds.Length)];
			audioSource.Play();
			yield return new WaitForSecondsRealtime(audioSource.clip.length);
			SceneManager.LoadScene(0);
        }
        else // assume is enemy for now, reconsider on other NPCs
        {
            var enemyAI = GetComponent<EnemyAI>();
            DestroyObject(gameObject, deathVanishSeconds);
        }
	}
}
