using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//NOTICE: The gameobject with this script attached MUST have a audio source attached for it to function properly
[RequireComponent(typeof(AudioSource))]
public class PlayMusic : MonoBehaviour
{
    [Tooltip("Requires Audio Source Component, can be empty")]
    public AudioSource src;//can have no audio clip
    [Header("Sound Selection")]
    public List<AudioClip> musicClips = new List<AudioClip>();//the selection of sounds that the object is able to play
    public int selectedClip = 0;
    [Tooltip("Playing the selected sound?")]
    public bool playing = false;
    public bool fadedOut = true;

    // Start is called before the first frame update
    void Start()
    {
        src = this.GetComponent<AudioSource>();//get the component from the object this is on if it exists
        src.loop = true;//make audio loop
        if (musicClips == null) //if no entries in list
            musicClips.Add(src.clip);//add one from the source
    }
    // Update is called once per frame
    void Update()
    {
        if (playing == true && src.isPlaying == false)
        {
            if (musicClips.Count > selectedClip) src.PlayOneShot(musicClips[selectedClip]);//play it
            else Debug.Log("Tried playing clip outside of range");
        }
        if (playing == false)
        {
            StartCoroutine(StartAudioFade());
        }
        if (fadedOut)
        {
            src.Stop();
        }
    }

    public IEnumerator StartAudioFade()
    {
        float currentTime = 0;
        float start = src.volume;

        while (currentTime < src.clip.length)
        {
            currentTime += Time.deltaTime;
            src.volume = Mathf.Lerp(start, 0, currentTime / src.clip.length);
            yield return null;
        }
        fadedOut = true;
        Debug.Log("E");
        yield break;
    }
}
