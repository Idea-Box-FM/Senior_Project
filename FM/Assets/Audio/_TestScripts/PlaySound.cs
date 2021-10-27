using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//NOTICE: The gameobject with this script attached MUST have a audio source attached for it to function properly

public class PlaySound : MonoBehaviour
{
    public AudioSource src;//can have no audio clip
    public List<AudioClip> soundClips = new List<AudioClip>();
    public List<AudioClip> soundQueue = new List<AudioClip>();
    public int selectedClip = 0;
    public bool play;

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
        if (play)
        {
            src.clip = soundClips[selectedClip];//store last played sound clip
            play = false;
        }
        if (src.isPlaying == false)
            if (soundQueue.Count > 0)//if more than 1 entry
            {
                src.PlayOneShot(soundQueue[soundQueue.Count-1]);//play it
                soundQueue.Remove(soundQueue[soundQueue.Count-1]);//remove it
            }
    }
    public void Play(int clipID)
    {
        this.selectedClip = clipID;
        soundQueue.Add(soundClips[clipID]);
        soundQueue = soundQueue.Distinct().ToList();//remove dupes
        play = true;
    }
    public void Play(string clipName)//less efficent
    {
        soundClips.Find(i => i.name == clipName);//search for entry in list (Note: bigger list = slower)
        play = true;
    }
}
