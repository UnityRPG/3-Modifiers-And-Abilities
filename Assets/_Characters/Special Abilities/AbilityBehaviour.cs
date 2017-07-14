using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;

		Animator animator;
        AudioSource audioSource;
        const string ATTACK_TRIGGER = "Attack";
		const string DEFAULT_ATTACK_STATE = "DEFAULT ATTACK";

		void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        public abstract void Use(GameObject target = null);

        public void SetConfig(AbilityConfig configToSet)
		{
			config = configToSet;
		}

		protected void PlayParticleInWorldSpace()
        {
            SetupParticleEffect();
        }

		protected void PlayParticleInPlayerSpace()
		{
            var effect = SetupParticleEffect();
            effect.transform.parent = transform.parent;
		}

        private GameObject SetupParticleEffect()
        {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
            return prefab;
        }

        protected void PlayAbilityAnimation()
        {
            var animatorOverrideController = GetComponent<Character>().GetOverrideController();
			animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK_STATE] = config.GetAbilityAnimation();
            animator.SetTrigger(ATTACK_TRIGGER);
		}

        protected void PlayAbilitySound()
        {
            var abilitySounds = config.GetAbilitySounds();
            int randomIndex = Random.Range(0, abilitySounds.Length);
            print(randomIndex);
            audioSource.clip = abilitySounds[randomIndex];
			audioSource.Play();
        }
    }
}