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
        selectorScripts = GameObject.FindObjectsOfType<PlaySelector>();
    }

    public void ReadSDS()
    {
        for (int i = 0; i < selectorScripts.Length; i++)
        {
            if(selectorScripts[i].isSelected == true)
            {
                string content = selectorScripts[i].gameObject.GetComponentInParent<SDSInfo>().currentContent;
                //Use object contents.contents to find sds files in filemanager
                FileManager fileManager = FileManager.fileManager;
                string path = fileManager.FormatPath(fileManager.sdsPath, content);
                Application.OpenURL(path);
                //display SDS
            }
        }
    }

    public void DeselectObjects()
    {
        for (int i = 0; i < selectorScripts.Length; i++)
        {
            if (selectorScripts[i].isSelected == true)
            {
                selectorScripts[i].Deselect();
            }
        }
    }

}
