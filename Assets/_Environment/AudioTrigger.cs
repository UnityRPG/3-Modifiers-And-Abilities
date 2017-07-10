﻿using UnityEngine;
using RPG.Characters;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] bool triggerOnPlayerOnly = true;
    [SerializeField] float triggerRadius = 0f;
    [SerializeField] bool isOneTimeOnly = true;

    [SerializeField] bool hasPlayed = false;
    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clip;

        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = triggerRadius;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggerOnPlayerOnly && other.GetComponent<PlayerControl>())
        {
            RequestPlayAudioClip();
        }
    }

    void RequestPlayAudioClip()
    {
        if (isOneTimeOnly && hasPlayed)
        {
            return;
        }
        else if (audioSource.isPlaying == false)
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}