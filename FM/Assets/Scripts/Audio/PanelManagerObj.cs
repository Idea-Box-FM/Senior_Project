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
    public Canvas canvas;//canvas, mostly for close functionality
    public List<GameObject> panels = new List<GameObject>();//all panels, mostly for using nextPanel functionality
    public int initalPanelID = 0;//the panel to activate first
    
    private Vector3 objStart;
    private string currentPanelName;//the current Panel as a string
    private int currentPanelID;//the current Panel as a ID
    [Header("Below are options for changing Panels, select a trigger type and Panel change option:", order = 0)]//display info about Panel

    //change Panel options
    [Header("Change Panel options (1 w/input or none):", order = 1)]
    public bool nextPanel = false;//next Panel based upon the build settings list
    public bool close = false;//close the panel
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
    private PlaySoundEffect soundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        currentPanelName = panels[initalPanelID].name;//get the Panel name as a string
        currentPanelID = initalPanelID;//get the Panel as an ID
        _ = "The current Panel is: " + currentPanelName + " with ID: " + currentPanelID + ", below are options for changing Panels: ";//information about Panel

        if (changedPanelName == "") changedPanelName = "SamplePanel";//default

        TryGetComponent<PlaySoundEffect>(out PlaySoundEffect soundEffect);

        if (soundEffect != null)//if it isn't empty
            this.soundEffect = soundEffect;//assign
        else
            Debug.LogWarning("PlaySoundEffect not found on \"" + this.gameObject.name + "\" object");
    }

    // Update is called once per frame
    void Update()
    {
        if (instantChange)//if change by script
        {
            StartCoroutine(ChangePanelDelay());
            instantChange = false;//disable to prevent looping
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnEnter)
        {
            StartCoroutine(ChangePanelDelay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerOnExit)
        {
            StartCoroutine(ChangePanelDelay());
        }
    }

    public IEnumerator ChangePanelDelay(GameObject newPanel = null)
    {
        Debug.Log("E");
        yield return new WaitForSeconds(0.001f);//wait for millisecond for button to play
        if (soundEffect.src.isPlaying == true)
            StartCoroutine(DelayedChange(soundEffect.src.time, newPanel));
        else
            StartCoroutine(DelayedChange(0, newPanel));//instant change
    }

    IEnumerator DelayedChange(float waitS, GameObject panelToChangeTo = null)
    {
        bool executed = false;

        objStart = this.gameObject.transform.position;//store inital position of button

        yield return new WaitForSeconds(waitS);//delay

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
            SetPanel(newPanelName: changedPanelName);//change to the Panel that has the string
            executed = true;
        }
        if (panelToChangeTo != null)//change to the Panel by passing in it's object
        {
            SetPanel(newPanelName: panelToChangeTo.name);//change to the Panel that was passed in
            executed = true;
        }
        if (changeByID)
        {
            SetPanel(newPanelID: changedPanelID);//change to the Panel that has the ID
            executed = true;
        }
        if (close == true)
        {
            canvas.gameObject.SetActive(false);
            executed = true;
        }
        else if(executed == false)
        {
            Debug.LogError("No Panel change option selected on " + this.gameObject.name + " object!");
        }

        //after changing panel
        
    }

    public void SetPanel(int newPanelID = -1, string newPanelName = null)//use id or string
    {
        for (int i = 0; i < panels.Count; i++)//loop through all panels
        {
            panels[i].SetActive(false);//set them to false
            if (i == newPanelID)//if current panel's ID
            {
                currentPanelName = panels[newPanelID].name;//store current panel Name (get to keep consistent)
                panels[i].SetActive(true);//set it to true
            }
            if(panels[i].name == newPanelName)//if current panel's Name
            {
                currentPanelID = i;//store current panel ID (get to keep consistent)
                panels[i].SetActive(true);//set it to true
            }
        }
        this.currentPanelName = newPanelName;
        this.currentPanelID = newPanelID;
    }

    //button methods
    public void ChangePanel(GameObject newPanel = null)
    {
        StartCoroutine(ChangePanelDelay(newPanel));
    }
    public void ChangePanel()
    {
        StartCoroutine(ChangePanelDelay());
    }
}
