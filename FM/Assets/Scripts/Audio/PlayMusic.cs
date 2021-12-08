using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.SceneManagement;

//NOTICE: The gameobject with this script attached MUST have a audio source attached for it to function properly
[RequireComponent(typeof(AudioSource))]
public class PlayMusic : MonoBehaviour
{
    public enum MusicAction
    {
        Idle,
        FadeIn,
        FadeOut,
        Done
    }

    [Tooltip("Requires Audio Source Component, can be empty")]
    public AudioSource src;//can have no audio clip
    [Header("Sound Selection")]
    public List<AudioClip> musicClips = new List<AudioClip>();//the selection of sounds that the object is able to play
    public int selectedClip = 0;
    [Tooltip("Current music's status")]
    public MusicAction currentMusicAction = MusicAction.FadeIn;//fade in by default
    public bool currentlyPlaying = true;
    private bool storedLastValue;
    static bool changeableState;
    public static float fadeTime = 1f;//make publically available

    //!fix inital starting of music
    //condense toggling of values within function

    // Start is called before the first frame update
    void Start()
    {
        src = this.GetComponent<AudioSource>();//get the component from the object this is on if it exists
        src.loop = true;//make audio loop
        if (musicClips == null) //if no entries in list
            musicClips.Add(src.clip);//add one from the source
        currentlyPlaying = storedLastValue;
    }
    // Update is called once per frame
    void Update()
    {
        //test inputs
        if (Keyboard.current.spaceKey/*specific key*/.isPressed == true) {
            Debug.Log("Play");
            Play();
        }
        if (Keyboard.current.leftShiftKey/*specific key*/.isPressed == true)
        {
            Debug.Log("Stop");
            Stop();
        }
        if (Keyboard.current.leftCtrlKey/*specific key*/.wasReleasedThisFrame == true)
        {
            Debug.Log("Next");
            SceneChange();
        }

        UpdateCurrentState();
        UpdateAudioClip();
        PreformCurrentState();

        storedLastValue = currentlyPlaying;
    }

    public void UpdateCurrentState()
    {
        changeableState = currentMusicAction != MusicAction.FadeIn || currentMusicAction != MusicAction.FadeOut || currentMusicAction != MusicAction.Done;//store if changeable every frame

        if (currentlyPlaying != storedLastValue)//if value of bool updated
        {
            switch (currentlyPlaying)
            {
                case true:
                    currentMusicAction = MusicAction.FadeIn;
                    break;
                case false:
                    currentMusicAction = MusicAction.FadeOut;
                    break;
            }
        }
    }

    private void UpdateAudioClip()
    {
        if (src.isPlaying == false)//if is not already playing something else
        {
            if (selectedClip <= musicClips.Count)//if within range
                src.clip = musicClips[selectedClip];//add it to source to allow editing
            else Debug.LogError("Tried playing clip outside of range");//otherwise show error
        }
    }

    private void PreformCurrentState()
    {
        if (src.clip != null)//if there is a sound clip within the slot
        {
            switch (currentMusicAction)
            {
                case MusicAction.FadeIn://if want to play music and idle
                    StartCoroutine(AudioFade(MusicAction.FadeIn, fadeTime));//fade in audio
                    break;
                case MusicAction.FadeOut://if want to stop playing music and idle
                    StartCoroutine(AudioFade(MusicAction.FadeOut, fadeTime));//fade out audio
                    break;
                case MusicAction.Done://if done with task
                    currentMusicAction = MusicAction.Idle;//reset after completing task
                    break;
                default:
                    //Debug.Log("No action!");
                    break;
            }
        }
        else { /*Debug.Log("No audio clip!");*/ }
    }

    public IEnumerator AudioFade(MusicAction action, float duration)
    {
        float currentTime = 0;
        float start = src.volume;
        float targetVolume = start;//keep same by default

        //Debug.Log("Action: " + action);

        if (action == MusicAction.FadeIn && src.isPlaying == false)//if fading in and is not playing
        {
            src.Play();//play
        }
        else{/*Debug.Log("Could not call play, still in fade state");*/}

        switch (action)//determine target
        {
            case MusicAction.FadeIn:
                targetVolume = 1;
                break;
            case MusicAction.FadeOut:
                targetVolume = 0;
                break;
            default:
                targetVolume = .5f;
                break;
        }

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            src.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            //Debug.Log("Action: " + action + ", Time: " + currentTime);
            yield return null;
        }

        if (action == MusicAction.FadeOut && src.isPlaying == true)//if fading out and is playing
        {
            src.Pause(); //pause after fading out
        }

        if(action == MusicAction.FadeIn)
        {
            currentlyPlaying = true;
        }

        //Debug.Log("Reached requested audio volume");
        currentMusicAction = MusicAction.Done;

        yield break;
    }

    public void Play()
    {
        if (changeableState == true)
            currentlyPlaying = true;
        else
            Debug.Log("Unable to change state to play");
    }
    public void Stop()
    {
        if (changeableState == true)
            currentlyPlaying = false;
        else
            Debug.Log("Unable to change state to stop");
    }
    public void SceneChange()//plays the next music clip
    {
        //process: fade out, change selected clip, fade in
        //fade out music
        currentMusicAction = MusicAction.FadeOut;

        //change scene and clip
        selectedClip++;
        if (selectedClip > musicClips.Count - 1)//if outside of range
        {
            selectedClip = 0;//reset
        }

        StartCoroutine(WaitAndFadeIn(fadeTime*2));
    }

    public IEnumerator WaitAndFadeIn(float timeS)
    {
        yield return new WaitForSeconds(timeS);

        //fade in music
        currentMusicAction = MusicAction.FadeIn;
    }
    
}
