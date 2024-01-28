using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using GGJ.Ingame.Common;

public class AudioManger : SingletonBehaviour<AudioManger>
{
    AudioSource audio;
    public AudioClip GroundEnemyFootsteps;
    public AudioClip FlyingEnemyFootsteps;
    public AudioClip DestroyDefense;
    public AudioClip Draw;
    public AudioClip GetPoint;
    public AudioClip Superattack;
    public AudioClip Clearance;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void Play(string name, bool isloop)
    {
        var clip = GetAudioClip(name);
        if (clip != null)
        {
            audio.clip = clip;
            audio.loop = isloop;
            audio.Play();
        }

        AudioClip GetAudioClip(string name)
        {
            switch (name)
            {
                case "GroundEnemyFootsteps":
                    return GroundEnemyFootsteps;
                case "FlyingEnemyFootsteps":
                    return FlyingEnemyFootsteps;
                case "DestroyDefense":
                    return DestroyDefense;
                case "Draw":
                    return Draw;
                case "GetPoint":
                    return GetPoint;
                case "Superattack":
                    return Superattack;
                case "Clearance":
                    return Clearance;
            }
            return null;
        }

       
    }
}
