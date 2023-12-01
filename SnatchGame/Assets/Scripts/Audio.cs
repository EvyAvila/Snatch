using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class Audio : MonoBehaviour
{
    [SerializeField]
    public AudioSource audioName { get; private set; }

    [SerializeField]
    float volume;

    [SerializeField]
    bool loop;

    private void Awake()
    {
        this.audioName = this.gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        this.audioName.volume = volume;
        this.audioName.loop = loop;
    }

    public void PlayAudio()
    {
        this.audioName.Play();
    }

    public void StopAudio()
    {
        this.audioName.Stop();
    }

    public void PlayStart()
    {
        this.audioName.PlayOneShot(this.audioName.clip);
    }

}


