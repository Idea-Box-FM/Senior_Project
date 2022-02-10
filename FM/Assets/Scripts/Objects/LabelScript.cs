using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelScript : MonoBehaviour
{
    string SDS;
    public Selector[] objects;
    int content;

    // Start is called before the first frame update
    void Start()
    {
        SDS = null;
       // objects = GameObject.FindGameObjectsWithTag("FMPrefab");
    }

    // Update is called once per frame
    void Update()
    {
        // objects = GameObject.FindGameObjectsWithTag("FMPrefab");
        objects = GameObject.FindObjectsOfType<Selector>();

        for (int i = 0;i < objects.Length;i++)
        {
            if(objects[i].GetComponent<Selector>().isSelected == true)
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
