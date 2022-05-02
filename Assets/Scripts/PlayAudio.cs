using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip inaDeath;
    [SerializeField] private AudioClip kiaraDeath;
    [SerializeField] private AudioClip ameDeath;
    [SerializeField] private AudioClip calliDeath;
    [SerializeField] private AudioClip guraDeath;

    public void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.Play();
    }

    public void PlayInaDeath()
    {
        audioSource.clip = inaDeath;
        audioSource.Play();
    }

    public void PlayKiaraDeath()
    {
        audioSource.clip = kiaraDeath;
        audioSource.Play();
    }

    public void PlayAmeDeath()
    {
        audioSource.clip = ameDeath;
        audioSource.Play();
    }

    public void PlayCalliDeath()
    {
        audioSource.clip = calliDeath;
        audioSource.Play();
    }

    public void PlayGuraDeath()
    {
        audioSource.clip = guraDeath;
        audioSource.Play();
    }
}
