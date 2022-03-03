using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;



/*Flower Box
 * 
 * Intention: To collect all of the objects in the scene and find the ones that are selected 
 * 
 * Editor: Tyler Rubenstein
 *   Added to main 2/7/22
 *   
 * Editor: Pat Naatz
 *  replaced ObjectContents with SDSInfo 2/24/2022
 *  Fixed bug where SDSInfo was on parent 2/24/2022
 */

public class LabelScript : MonoBehaviour
{

    public Selector[] objects; 
    int content;

    [Header("SDS Button List")]
    public string[] sdsList;
    public GameObject itemtemplate;
    public GameObject buttonList;
    GameObject s;
    char[] rtfTrim = { '.', 'r', 't', 'f' };





    // Start is called before the first frame update
    void Start()
    {
        sdsList = FileManager.fileManager.sdsFiles;
        UpdateSdsList();
    }

    // Update is called once per frame
    void Update()
    {
        objects = GameObject.FindObjectsOfType<Selector>();
        
    }

    /// <summary>
    /// creates the list of buttons for changing the sds of an object
    /// </summary>
    public void UpdateSdsList()
    {
        for(int i = 0; i < sdsList.Length; i++)
        {
            s = Instantiate(itemtemplate, buttonList.transform);
            string buttonName = sdsList[i].TrimEnd(rtfTrim);
            s.transform.GetChild(0).GetComponent<TMP_Text>().text = buttonName;
           // s.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.blue;
            s.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }
    }


    void ItemClicked(int itemIndex)
    {
       // Debug.Log("Button " + itemIndex + " was clicked");
        ChangeOption(itemIndex);

        SelectObject();

        
    }

    /// <summary>
    /// loops through list of selected objects and if any of them
    /// are true then change its sds to the one that is selected
    /// </summary>
    void SelectObject()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<Selector>().isSelected == true)
            {
               //change sds
                objects[i].GetComponentInParent<SDSInfo>().ChangeContents(content);
            }

        }
    }

    /// <summary>
    /// Deselects whatever object is selected
    /// </summary>
    public void DeselectObject()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<Selector>().Deselect();

        }
    }

    /// <summary>
    /// CHange the selected index in the array of sds
    /// </summary>
    /// <param name="m"> index of sds</param>
    public void ChangeOption(int m)
    {
        content = m;
    }



}
