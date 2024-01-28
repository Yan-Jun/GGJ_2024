using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    List<AudioSource> audios = new List<AudioSource>();
    public AudioClip GroundEnemyFootsteps;
    public AudioClip FlyingEnemyFootsteps;
    public AudioClip DestroyDefense;
    public AudioClip Draw;
    public AudioClip GetPoint;
    public AudioClip Superattack;
    public AudioClip Clearance;
    void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }

    void Play(int index, string name, bool isloop)
    {
        var clip = GetAudioClip(name);
        if (clip != null)
        {
            var audio = audios[index];
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
