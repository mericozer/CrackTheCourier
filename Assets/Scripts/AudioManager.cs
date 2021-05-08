using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;

    public bool inGame;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
    }
    void Start()
    {
        //background music
        Play("CrackMain");
        
        if(inGame)
        {Play("Drive");}
        
        //to play from another script use like:
        // FindObjectOfType<AudioManager>().Play("CannonSound");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audioName);
        s.source.Play();
    }

    public void Switch(string currentAudio, string nextAudio)
    {
        Sound s = Array.Find(sounds, sound => sound.name == currentAudio);
        s.source.Stop();

        Sound n = Array.Find(sounds, sound => sound.name == nextAudio);
        n.source.Play();

    }
    public void Stop(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audioName);
        s.source.Stop();
    }
}
