using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//Not my code - Jeff Meyers
public enum KeyPlayMode { PlayOnKey, PlayOneShotOnKey, LoopOnKey, PlayOnStart, PlayOnAwake, LoopOnStart, LoopOnAwake }
public enum KeyPriority { Normal, High, HighWithSkip }

public enum Screen { Menu, Game, Loss, Won }

public class Audio : MonoBehaviour
{
    public AudioClip Clip;
    public AudioSource Source;
    public KeyCode Key;

    public KeyPlayMode keyPlayMode;
    public KeyPriority keyPriority;

    public Screen screen;

    private bool isPaused;

    private PlayerUI player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("BasePlayer").GetComponent<PlayerUI>();

        this.Source.clip = Clip;
        this.Source.name = Clip.name;
        switch (keyPlayMode)
        {
            case KeyPlayMode.PlayOnStart:
                Source.PlayOneShot(Clip);
                break;
            case KeyPlayMode.LoopOnStart:
                Source.loop = true;
                Source.Play();
                break;
        }
    }

    void Awake()
    {
        this.Source = this.gameObject.AddComponent<AudioSource>();
        if (this.keyPriority == KeyPriority.High)
        {
            this.Source.priority = 1;

        }
        if (this.keyPriority == KeyPriority.HighWithSkip)
        {
            this.Source.priority = 1;
            this.Source.bypassEffects = true;
            this.Source.bypassListenerEffects = true;
            this.Source.bypassReverbZones = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (screen)
        {
            case Screen.Game:
            case Screen.Menu:

                if (keyPlayMode == KeyPlayMode.LoopOnKey && player.playerState == PlayerState.Active)
                {
                    Source.loop = true;
                    Source.clip = Clip;
                    if (Input.GetKeyDown(Key))
                    {
                        if (isPaused)
                        {
                            Source.UnPause();
                            this.isPaused = false;
                        }
                        else
                            Source.Play();
                    }
                }
                else if (player.playerState == PlayerState.Win || player.playerState == PlayerState.Lose)
                {
                    Source.Stop();
                }

                break;
            case Screen.Loss:
                if (player.playerState == PlayerState.Lose && keyPlayMode == KeyPlayMode.PlayOnKey)
                {
                   
                    //Source.Play();
                }
                keyPlayMode = KeyPlayMode.PlayOnStart;

                break;

            case Screen.Won:
                if (player.playerState == PlayerState.Win)
                {
                    keyPlayMode = KeyPlayMode.PlayOnStart;
                }
               

                break;
            
        }



    }
}


