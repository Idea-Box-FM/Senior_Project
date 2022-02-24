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
        if (tutorialSections.Count - 1 == currentSectionID)//if last section in list
        {
            ended = true;
        }
    }

    public void NextSection()
    {
        if (tutorialSections.Count - 1 != currentSectionID)//if not last section in list
        {
            currentSectionID++;//next section
            if (currentSectionID > tutorialSections.Count - 1)//if outside of list
                currentSectionID = 0;
        }

        SetSection(newSectionID: currentSectionID);
    }
    public void PreviousSection()
    {
        if(ended == false)//if not last section in list
            currentSectionID--;

        SetSection(newSectionID: currentSectionID);
    }

    public void SetSection(int newSectionID = -1)//use id or string
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
