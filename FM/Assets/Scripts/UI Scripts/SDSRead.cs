using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Editor: Pat Naatz
 *  replaced ObjectContents with SDSInfo 2/24/2022
 */

public class SDSRead : MonoBehaviour
{
    public PlaySelector[] selectorScripts;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ReadSDS()
    {
        selectorScripts = GameObject.FindObjectsOfType<PlaySelector>();

        for (int i = 0; i < selectorScripts.Length; i++)
        {
            if(selectorScripts[i].isSelected == true)
            {
                //create sds file path
                string content = selectorScripts[i].GetComponentInParent<SDSInfo>().currentContent;
                FileManager fileManager = FileManager.fileManager;
                string path = fileManager.FormatPath(fileManager.sdsPath, content);

                //display SDS
                Application.OpenURL(path);
            }
        }
    }

    public void DeselectObjects()
    {
        selectorScripts = GameObject.FindObjectsOfType<PlaySelector>();

        for (int i = 0; i < selectorScripts.Length; i++)
        {
            if (selectorScripts[i].isSelected == true)
            {
                selectorScripts[i].Deselect();
            }
        }
    }

}
