using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for playing audio in games.
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

    //Plays background music for Ina character.
    public void PlayInaMusic()
    {
        audioSource.clip = inaMusic;
        audioSource.Play();
    }

    //Plays background music for Kiara character.
    public void PlayKiaraMusic()
    {
        audioSource.clip = kiaraMusic;
        audioSource.Play();
    }

    //Plays background music for Ame character.
    public void PlayAmeMusic()
    {
        audioSource.clip = ameMusic;
        audioSource.Play();
    }

    //Plays background music for Calli character.
    public void PlayCalliMusic()
    {
        audioSource.clip = calliMusic;
        audioSource.Play();
    }

    //Plays background music for Gura character.
    public void PlayGuraMusic()
    {
        audioSource.clip = guraMusic;
        audioSource.Play();
    }

    //Plays the intro music for a round.
    public void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.Play();
    }

    //Plays the death sound for Ina character.
    public void PlayInaDeath()
    {
        audioSource.clip = inaDeath;
        audioSource.Play();
    }

    //Plays the death sound for Kiara character.
    public void PlayKiaraDeath()
    {
        audioSource.clip = kiaraDeath;
        audioSource.Play();
    }

    //Plays the death sound for Ame character.
    public void PlayAmeDeath()
    {
        audioSource.clip = ameDeath;
        audioSource.Play();
    }

    //Plays the death sound for Calli character.
    public void PlayCalliDeath()
    {
        audioSource.clip = calliDeath;
        audioSource.Play();
    }

    //Plays the death sound for Gura character.
    public void PlayGuraDeath()
    {
        audioSource.clip = guraDeath;
        audioSource.Play();
    }

    //Sets if music should loop or not.
    public void SetLoop(bool toLoop)
    {
        loop = toLoop;
    }

    //Stops music.
    public void StopMusic()
    {
        audioSource.Stop();
    }

    //If music is currently playing.
    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    //Resumes paused music.
    public void UnPause()
    {
        audioSource.UnPause();
    }

    //Pauses music.
    public void Pause()
    {
        audioSource.Pause();
    }
}
