using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public List<GameObject> tutorialSections = new List<GameObject>();
    public int currentSectionID;//the current section as a ID
    public bool ended = false;
    public bool hidden = false;

    public static Tutorial tutorial;

    // Start is called before the first frame update
    void Start()
    {
        if (tutorialSections.Count < 1)
            Debug.LogError("No tutorial sections in \"" + this.gameObject.name + "\" object's list");
        else
        {
            if (tutorialSections[tutorialSections.Count - 1] != null)//if last element is not empty
                tutorialSections.Add(null);//add one in case

            SetSection(currentSectionID);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ended == true)
        {
            hidden = true;//make hidden
            //loop through and disable all
            for (int i = 0; i < tutorialSections.Count; i++)//loop through all tutorial sections
            {
                if (tutorialSections[i] != null)//if it is not empty
                {
                    tutorialSections[i].SetActive(false);//set them to false
                }
            }
            currentSectionID = 0;//reset
        }

        tutorialSections[currentSectionID].SetActive(!hidden);
    }

    public void NextSection()
    {
        if (tutorialSections.Count - 1 != currentSectionID)//if not last section in list
        {
            currentSectionID++;//next section
            if (currentSectionID > tutorialSections.Count - 1)//if outside of list
                currentSectionID = 0;//loop back
        }
        if(tutorialSections.Count - 1 == currentSectionID)//if last section in list (empty)
        {
            ended = true;//end the tutorial
        }

        SetSection(newSectionID: currentSectionID);//set the proper section
    }
    

    public void SetSection(int newSectionID)//use id or string
    {
        if(ended == false)
        {
            for (int i = 0; i < tutorialSections.Count; i++)//loop through all tutorial sections
            {
                if (tutorialSections[i] != null)//if it is not empty
                {
                    tutorialSections[i].SetActive(false);//set them to false

                    if (i == newSectionID)//if current Section's ID
                    {
                        tutorialSections[i].SetActive(true);//set it to true
                    }
                }
            }

            this.currentSectionID = newSectionID;
        }
    }

    public void ResetTutorial(int newSectionID = 0)
    {
        hidden = false;
        ended = false;
        SetSection(newSectionID);
    }

    public void SetEnded(bool newEnded)
    {
        ended = newEnded;
    }
    public void SetHidden(bool newHidden)
    {
        hidden = newHidden;
    }
}
