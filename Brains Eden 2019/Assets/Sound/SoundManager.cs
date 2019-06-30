using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer Mixer;
    public AudioSource Source;
    public AudioClip Music;

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(gameObject);

        Source.clip = Music;
        Source.Play();
    }
    
}
