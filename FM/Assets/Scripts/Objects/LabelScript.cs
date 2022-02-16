using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;



public class LabelScript : MonoBehaviour
{
    string SDS;
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
        SDS = null;
        // objects = GameObject.FindGameObjectsWithTag("FMPrefab");
        sdsList = FileManager.fileManager.sdsFiles;
        UpdateSdsList();
    }

    // Update is called once per frame
    void Update()
    {
        // objects = GameObject.FindGameObjectsWithTag("FMPrefab");
        objects = GameObject.FindObjectsOfType<Selector>();

       

        
        
    }

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
        Debug.Log("Button " + itemIndex + " was clicked");
        ChangeOption(itemIndex);

        SelectObject();

        
    }

    void SelectObject()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<Selector>().isSelected == true)
            {
                //change sds
                //objects[i].Label = SDS
                objects[i].GetComponent<ObjectContents>().ChangeContents(content);

            }

        }
    }



    public void ChangeOption(int m)
    {
        content = m;
    }



}
