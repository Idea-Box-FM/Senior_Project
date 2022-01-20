using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Notes:
 * Script to be placed on gameobjects with triggers or to be triggered externally to make the Panel change
 * Make sure to have all Panels in the build settings
*/

public class PanelManagerObj : MonoBehaviour
{
    //current Panel options
    public List<GameObject> panels = new List<GameObject>();
    public int initalPanelID = 0;//the first activated panel

    private string currentPanelName;//the current Panel as a string
    private int currentPanelID;//the current Panel as a ID
    [Header("Below are options for changing Panels, select a trigger type and Panel change option:", order = 0)]//display info about Panel

    //change Panel options
    [Header("Change Panel options (1 w/input or none):", order = 1)]
    public bool nextPanel = false;//next Panel based upon the build settings list
    public bool exit = false;//Quit the application
    //input new Panel location via name
    public bool changeByName = false;//change the Panel via the name of the Panel
    public string changedPanelName;//Panel to change to as string
    //input new Panel location via id
    public bool changeByID = false;//change the Panel via the ID of the Panel
    public int changedPanelID;//Panel to change to as ID
    //trigger options
    [Header("Trigger options (1 or none):")]
    public bool triggerOnEnter = false;//when entering a trigger, cause the Panel to change based upon the above settings
    public bool triggerOnExit = false;//when exiting a trigger, cause the Panel to change based upon the above settings
    public bool instantChange = false;//bool to be triggered by external scripts

    // Start is called before the first frame update
    private void Start()
    {
        currentPanelName = panels[initalPanelID].name;//get the Panel name as a string
        currentPanelID = initalPanelID;//get the Panel as an ID
        _ = "The current Panel is: " + currentPanelName + " with ID: " + currentPanelID + ", below are options for changing Panels: ";//information about Panel

        if (changedPanelName == "") changedPanelName = "SamplePanel";//default


    }

    // Update is called once per frame
    void Update()
    {
        if (instantChange)//if change by script
        {
            ChangePanel();
            instantChange = false;//disable to prevent looping
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnEnter)
        {
            ChangePanel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerOnExit)
        {
            ChangePanel();
        }
    }

    public void ChangePanel(bool executed = false)
    {
        StartCoroutine(DelayedChange(PlayMusic.fadeTime * 1.5f, executed));
    }

    IEnumerator DelayedChange(float waitS, bool executed = false)
    { 
        this.GetComponent<PlayMusic>().Stop();

        yield return new WaitForSeconds(waitS);

        //check if object still exists after Panel transition
        if (nextPanel)
        {
            int newIndex = currentPanelID + 1;//set new index value
            if (newIndex > currentPanelID - 1)//if over limit
                newIndex = 0;//loop back to zero
            SetPanel(newIndex);//go to the next Panel in the build settings
            executed = true;
        }
        if (changeByName)
        {
            SetPanel(newPanelName:changedPanelName);//change to the Panel that has the string
            executed = true;
        }
        if (changeByID)
        {
            SetPanel(newPanelID:changedPanelID);//change to the Panel that has the ID
            executed = true;
        }
        if (exit)
        {
            Application.Quit();//quit the application
            executed = true;
        }
        else if (executed == false)
        {
            Debug.LogError("No Panel change option selected on " + this.gameObject.name + " object!");
        }
    }

    public void SetPanel(int newPanelID = -1, string newPanelName = null)//use id or string
    {
        for (int i = 0; i < panels.Count; i++)//loop through all panels
        {
            panels[i].SetActive(false);//set them to false
            if (i == newPanelID)//if current panel
            {
                panels[i].SetActive(true);//set it to true
            }
            if(panels[i].name == newPanelName)
            {
                panels[i].SetActive(true);//set it to true
            }
        }

        this.currentPanelName = panels[newPanelID].name;
        this.currentPanelID = newPanelID;
    }

    //button methods

    public void SetPanelBtn(GameObject newPanel)
    {
        SetPanel(newPanelName:newPanel.name);

        ChangePanel(true);
    }
}
