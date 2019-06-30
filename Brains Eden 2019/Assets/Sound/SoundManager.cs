using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer Mixer;
    public AudioSource Source;
    public AudioClip Music;

    public AudioSource Effects;
    public AudioClip FireBall;
    public AudioClip ForcePush;
    public AudioClip Explosion;

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(gameObject);

        Source.clip = Music;
        Source.Play();
    }

    public void PlayFireball() {
        Effects.PlayOneShot(FireBall);
    }

    public void PlayForcePush() {
        Effects.PlayOneShot(ForcePush);
    }

    public void PlayExplosion(Vector3 position) {
        AudioSource.PlayClipAtPoint(Explosion, position);
        
    }
    
}
