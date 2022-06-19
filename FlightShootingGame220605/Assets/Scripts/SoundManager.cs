using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst { get; private set; }
    public AudioSource sfx;
    public AudioSource bgm;

    public AudioClip[] stageBgm;
    public AudioClip[] effects;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
    }

    public void BGMPlay(int i)
    {
        bgm.clip = stageBgm[i];
        bgm.Play();
    }

    public void SFXPlay(int i)
    {
        sfx.clip = effects[i];
        sfx.PlayOneShot(sfx.clip);
    }
}
