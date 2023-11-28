using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    public AudioClip[] Clips;
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
        
        this.Source.clip = Clips[0];
        this.Source.name = Clips[0].name;
        switch (keyPlayMode)
        {
            case KeyPlayMode.PlayOnStart:
                Source.PlayOneShot(Clips[0]);
                break;
            case KeyPlayMode.LoopOnStart:
                Source.loop = true;
                Source.Play();
                break;
        }
    }

    void Awake()
    {
        player = GameObject.Find("BasePlayer").GetComponent<PlayerUI>();

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

    private void Update()
    {
        
        switch(screen)
        {
            case Screen.Menu:
               // break;
            case Screen.Game:
                if(keyPlayMode == KeyPlayMode.LoopOnStart && player.playerState == PlayerState.Active)
                {
                    if(Source.clip != Clips[0])
                    {
                        Source.clip = Clips[0];
                        //Source.name = Clips[0].name;
                        Source.Play();
                       
                    }
                    
                }
                else
                {
                    Source.Stop();
                    if (player.playerState == PlayerState.Win)
                    {

                        this.Source.clip = Clips[2];
                        //this.Source.name = Clips[2].name;
                        screen = Screen.Won;

                    }
                    else if (player.playerState == PlayerState.Lose)
                    {

                        this.Source.clip = Clips[1];
                        //this.Source.name = Clips[1].name;
                        screen = Screen.Loss;
                    }
                }
                
                break;
            case Screen.Won:
                Source.PlayOneShot(Clips[2]);

                if(player.playerState == PlayerState.Active)
                {
                    screen = Screen.Game;
                }
                break;
            case Screen.Loss:
                Source.PlayOneShot(Clips[1]);
                break;

        }
        
    }
    
    public void StealObjectSound()
    {
        //Source.clip = Clips[6];
        //Source.name = Clips[6].name;
        Source.PlayOneShot(Clips[6]);
    }

}
