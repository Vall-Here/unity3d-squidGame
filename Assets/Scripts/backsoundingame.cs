using System;
using System.Collections;
using UnityEngine;

public class backsoundingame : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip soundKontol;
    public AudioClip backsoundTegang;

    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public event Action OnAudioFinish;


    private void Awake() {
        audioSource1 = GetComponent<AudioSource>();
    }


    public void PlayStart(){
        audioSource2.PlayOneShot(soundKontol);
        StartCoroutine(PlayStartDelay());
    }
    IEnumerator PlayStartDelay(){
        yield return new WaitForSeconds(soundKontol.length);
        OnAudioFinish?.Invoke();
        PlayTegang();
    }

    public void PlayTegang(){
        print("PlayTegang");
        audioSource1.clip = backsoundTegang;
        audioSource1.Play();
    }
}
