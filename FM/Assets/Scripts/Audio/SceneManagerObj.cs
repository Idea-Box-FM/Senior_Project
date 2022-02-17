using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*Notes:
 * Script to be placed on gameobjects with triggers or to be triggered externally to make the scene change
 * Make sure to have all scenes in the build settings
*/

public class SceneManagerObj : MonoBehaviour
{
    public PlayMusic musicPlayer;

    //current scene options
    private string currentSceneName;//the current scene as a string
    private int currentSceneID;//the current scene as a ID
    [Header("Below are options for changing scenes, select a trigger type and scene change option:", order=0)]//display info about scene

    //change scene options
    [Header("Change scene options (1 w/input or none):", order=1)]
    public bool nextScene = false;//next scene based upon the build settings list
    public bool exit = false;//Quit the application
    //input new scene location via name
    public bool changeByName = false;//change the scene via the name of the scene
    public string changedSceneName;//scene to change to as string
    //input new scene location via id
    public bool changeByID = false;//change the scene via the ID of the scene
    public int changedSceneID;//scene to change to as ID
    //trigger options
    [Header("Trigger options (1 or none):")]
    public bool triggerOnEnter = false;//when entering a trigger, cause the scene to change based upon the above settings
    public bool triggerOnExit = false;//when exiting a trigger, cause the scene to change based upon the above settings
    public bool instantChange = false;//bool to be triggered by external scripts

    // Start is called before the first frame update
    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;//get the scene name as a string
        currentSceneID = SceneManager.GetActiveScene().buildIndex;//get the scene as an ID
        _ = "The current scene is: " + currentSceneName + " with ID: " + currentSceneID + ", below are options for changing scenes: ";//information about scene

        if (changedSceneName == "") changedSceneName = "SampleScene";//default


    }

    // Update is called once per frame
    void Update()
    {
        if (instantChange)//if change by script
        {
            ChangeScene();
            instantChange = false;//disable to prevent looping
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnEnter)
        {
            ChangeScene();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerOnExit)
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        StartCoroutine(DelayedChange(PlayMusic.fadeTime));
    }

    IEnumerator DelayedChange(float waitS)
    {
        bool executed = false;

        if (musicPlayer.src.isPlaying)//if sound is playing
        {
            musicPlayer.Stop();

            yield return new WaitForSeconds(waitS);
        }

        //check if object still exists after scene transition
        if (nextScene)
        {
            int newIndex = SceneManager.GetActiveScene().buildIndex + 1;//set new index value
            if (newIndex > SceneManager.sceneCountInBuildSettings - 1)//if over limit
                newIndex = 0;//loop back to zero
            SceneManager.LoadScene(newIndex);//go to the next scene in the build settings
            executed = true;
        }
        if (changeByName)
        {
            SceneManager.LoadScene(changedSceneName);//change to the scene that has the string
            executed = true;
        }
        if (changeByID)
        {
            SceneManager.LoadScene(changedSceneID);//change to the scene that has the ID
            executed = true;
        }
        if (exit)
        {
            Application.Quit();//quit the application
            executed = true;
        }
        else if (executed == false)
        {
            Debug.LogError("No scene change option selected on " + this.gameObject.name + " object!");
        }
    }
}
