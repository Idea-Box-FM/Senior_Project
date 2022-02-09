using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

//NOTICE: The gameobject with this script attached MUST have an audio source for it to function properly
//[RequireComponent(typeof(AudioSource))]
public class PlaySoundEffect : MonoBehaviour
{
    [Tooltip("Requires Audio Source Component, can be empty")]
    public AudioSource src;//can have no audio clip
    [Header("Sound Selection")]
    public List<AudioClip> soundClips = new List<AudioClip>();//the selection of sounds that the object is able to play
    public int selectedClip = 0;
    [Tooltip("Play sound regardless of last played sound's state?")]
    public bool playOver;
    [Tooltip("Play the selected sound?")]
    public bool play;
    
    [Tooltip("Object Based Sound Queue")]
    public List<AudioClip> soundEffectQueueDisplay = new List<AudioClip>();//a list of sounds for this object to play
    [Tooltip("Object Based Sound Queue")]
    public static List<AudioClip> soundEffectQueue = new List<AudioClip>();//a list of sounds for this object to play

    [Header("Options")]
    [Tooltip("The name of the options slider game object, is a key")]
    public string optionSliderName;
    [Tooltip("How high the volume can be set, 1 is limit (100% volume)")]
    public float volumeLimit = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (src == null)//if source is not assigned
        {
            bool foundSrc = this.TryGetComponent<AudioSource>(out src);//get the component from the object this is on if it exists
            //if (foundSrc == false)//if it is still not found
            //    Debug.LogError("Did not assign \"src\" in the \"" + this.gameObject.name + "\" game object");//log an error
        }
        if (soundClips == null) //if no entries in list
            soundClips.Add(src.clip);//add one from the source
    }
    // Update is called once per frame
    void Update()
    {
        //get options
        if (optionSliderName != "")
        {
            if (GetOptionsStored.prefKeys.Count > 0)//if there are items in the list
            {
                int index = GetOptionsStored.prefKeys.IndexOf(optionSliderName);//find index, will return -1 if not found
                if (index < GetOptionsStored.prefValues.Count && index >= 0)//if is within amount of list and is found
                {
                    volumeLimit = float.Parse((string)GetOptionsStored.prefValues[index]) / 100;//get volume from options
                    src.volume = volumeLimit;//set the value
                }
                else
                    Debug.Log("Unable to find " + optionSliderName + " in current list of keys");
            }
        }
        else
            Debug.LogError("Assign optionSlider on \"" + gameObject.name + "\" game object");

        soundEffectQueueDisplay = soundEffectQueue;
        if (play == true)
        {
            AddToQueue(soundClips[selectedClip], playOver);
            //soundEffectQueueDisplay = soundEffectQueueDisplay.Distinct().ToList();//remove dupes
        }

        if (soundEffectQueue.Count > 0)//if more than 1 entry
        {
            if (src.isPlaying == false) { 
                src.PlayOneShot(soundEffectQueue[0]);//play it
                soundEffectQueue.Remove(soundEffectQueue[0]);//remove it
                play = false;
            }
        }
    }
    /// <summary>
    /// Play sound effect
    /// </summary>
    /// <param name="soundClip">The specific sound clip to play, does not need to be within the instance</param>
    /// <param name="playOver">Play over other sounds without waiting in a queue?</param>
    /// <param name="caller">The name of the object signaling for a sound to play</param>
    public void Play(AudioClip soundClip = null, bool playOver = true, string caller = "")
    {
        src.volume = volumeLimit;//set volume

        if (soundClip == null)//if default
            soundClip = soundClips[selectedClip];//assign current selected

        if (playOver == true)//if play over other sounds
        {
            //play manually
            src.PlayOneShot(soundClip);
            play = false;
        }
        if (playOver == false)//if wait in a queue
        {
            //play automatically
            play = true;//add to queue via bool
        }

        //if (caller != "") Debug.Log("Playing \"" + soundClip.name + "\" sound effect from \"" + caller + "\" game object");
    }
    /// <summary>
    /// Play sound effect, event varient (only allow up to one parameter to be in the list)
    /// </summary>
    /// <param name="id">The id of the sound within the list</param>
    public void Play(int id = 0)
    {
        if (id <= soundClips.Count - 1)
            Play(soundClips[id], caller: this.name/*comment out caller to pervent debugs*/);
        else
            Debug.LogError("ID (" + id + ") outside of given soundClips list (limit " + (soundClips.Count - 1) + ")");
    }
    /// <summary>
    /// Add entry to queue to play (use externally)
    /// </summary>
    /// <param name="soundClip">The specific sound clip to play, does not need to be within the instance</param>
    /// <param name="playOverOthers">Play over other sound effects?</param>
    public void AddToQueue(AudioClip soundClip, bool playOverOthers)//
    {
        if (playOverOthers == false)
            soundEffectQueue.Add(soundClip);
        if (playOverOthers == true)
            Play(soundClip);
    }
}
