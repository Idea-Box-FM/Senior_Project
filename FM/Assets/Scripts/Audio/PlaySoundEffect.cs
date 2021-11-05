using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

//NOTICE: The gameobject with this script attached MUST have a audio source attached for it to function properly
[RequireComponent(typeof(AudioSource))]
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

    // Start is called before the first frame update
    void Start()
    {
        src = this.GetComponent<AudioSource>();//get the component from the object this is on if it exists
        if (soundClips == null) //if no entries in list
            soundClips.Add(src.clip);//add one from the source
    }
    // Update is called once per frame
    void Update()
    {
        soundEffectQueueDisplay = soundEffectQueue;
        if (play == true)
        {
            AddToQueue(soundClips[selectedClip], playOver);
            //soundQueue = soundQueue.Distinct().ToList();//remove dupes
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
    public void Play(AudioClip soundClip)//play individual regardless of last sound state
    {
        src.PlayOneShot(soundClip);
        play = false;
    }
    public void AddToQueue(AudioClip soundClip, bool playOverOthers)//add entries (use externally)
    {
        if (playOverOthers == false)
            soundEffectQueue.Add(soundClip);
        if (playOverOthers == true)
            Play(soundClip);
    }
}
