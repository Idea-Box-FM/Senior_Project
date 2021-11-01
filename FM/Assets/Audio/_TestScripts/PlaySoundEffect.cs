using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<AudioClip> soundQueue = new List<AudioClip>();//a list of sounds for this object to play

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
        if (play == true)
        {
            AddToQueue(soundClips[selectedClip], playOver);
            //soundQueue = soundQueue.Distinct().ToList();//remove dupes
        }

        if (soundQueue.Count > 0)//if more than 1 entry
        {
            if (src.isPlaying == false) { 
                src.PlayOneShot(soundQueue[0]);//play it
                soundQueue.Remove(soundQueue[0]);//remove it
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
            soundQueue.Add(soundClip);
        if (playOverOthers == true)
            Play(soundClip);
    }
}
