using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip Backsound;
    public AudioClip GameOver;
    public AudioClip Jump;
    public AudioClip Scoring;
    public AudioClip Hurt;
    public AudioClip Win;
    public AudioClip SwordSlash;
    public AudioClip Slime;

    private void Start()
    {
        MusicSource.clip = Backsound;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
