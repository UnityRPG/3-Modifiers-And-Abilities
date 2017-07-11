using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using RPG.Characters; // to identify player for scene reload

public class HealthSystem : MonoBehaviour{

	[SerializeField] float maxHealthPoints = 100f;
	[SerializeField] Image healthBar = null;
	[SerializeField] AudioClip[] damageSounds;
	[SerializeField] AudioClip[] deathSounds;

	const string DEATH_TRIGGER = "Death";

	float currentHealthPoints;
    Animator animator;
    AudioSource audioSource;

	public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealthPoints = maxHealthPoints;
	}
	
	// Update is called once per frame
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

	public void AdjustHealth(float changePoints)
	{
        bool charaterDies = (currentHealthPoints - changePoints <= 0); // must ask before reducing health
		ReduceHealth(changePoints);
		if (charaterDies)
		{
			StartCoroutine(KillCharacter());
		}
	}

    IEnumerator KillCharacter()
    {
        animator.SetTrigger(DEATH_TRIGGER);
        //todo prevent character performing attack (or special) while dying

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
            DestroyObject(gameObject, enemyAI.GetDeathVanishDelay()); 
        }
	}

	private void ReduceHealth(float damage)
	{
		currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
		audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
		audioSource.Play();
	}
}
