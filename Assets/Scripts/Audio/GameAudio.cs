using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameAudio
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    public void PlayClip()
    {
        source.clip = clip;
        source.Play();
    }
}
