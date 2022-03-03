using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Editor: Pat Naatz
 *  replaced ObjectContents with SDSInfo 2/24/2022
 *  Updated script to work with new SelectorTool 3/3/2022
 */

public class SDSRead : MonoBehaviour
{
    public FMInfo[] selectorScripts;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ReadSDS()
    {
        selectorScripts = SelectorTool.SelectedObjects;

        for (int i = 0; i < selectorScripts.Length; i++)
        {
            if (selectorScripts[i].basePrefab is FMWithPrefab)
            {
                //create sds file path
                string content = selectorScripts[i].GetComponent<SDSInfo>().currentContent;
                FileManager fileManager = FileManager.fileManager;
                string path = fileManager.FormatPath(fileManager.sdsPath, content);

                //display SDS
                Application.OpenURL(path);
            }
        }
    }

    //public void DeselectObjects()
    //{
    //    selectorScripts = SelectorTool.selectorTool.selectedObjects.ToArray();

    //    for (int i = 0; i < selectorScripts.Length; i++)
    //    {
    //        if (selectorScripts[i].isSelected == true)
    //        {
    //            selectorScripts[i].Deselect();
    //        }
    //    }
    //}

}
