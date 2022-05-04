using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip inaMusic;
    [SerializeField] private AudioClip kiaraMusic;
    [SerializeField] private AudioClip ameMusic;
    [SerializeField] private AudioClip calliMusic;
    [SerializeField] private AudioClip guraMusic;
    [SerializeField] private AudioClip inaDeath;
    [SerializeField] private AudioClip kiaraDeath;
    [SerializeField] private AudioClip ameDeath;
    [SerializeField] private AudioClip calliDeath;
    [SerializeField] private AudioClip guraDeath;
    private bool loop;

    public void PlayInaMusic()
    {
        audioSource.clip = inaMusic;
        audioSource.Play();
    }

    public void PlayKiaraMusic()
    {
        audioSource.clip = kiaraMusic;
        audioSource.Play();
    }

    public void PlayAmeMusic()
    {
        audioSource.clip = ameMusic;
        audioSource.Play();
    }

    public void PlayCalliMusic()
    {
        audioSource.clip = calliMusic;
        audioSource.Play();
    }

    public void PlayGuraMusic()
    {
        audioSource.clip = guraMusic;
        audioSource.Play();
    }

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

    public void SetLoop(bool toLoop)
    {
        loop = toLoop;
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public void UnPause()
    {
        audioSource.UnPause();
    }

    public void Pause()
    {
        audioSource.Pause();
    }
}
