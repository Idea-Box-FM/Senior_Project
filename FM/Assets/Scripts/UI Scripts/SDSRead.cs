using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDSRead : MonoBehaviour
{
    //public ObjectContents objectContents;
    public PlaySelector[] selectorScripts;

    void Start()
    {
        //objectContents.contents = FileManager.fileManager.sdsFiles;

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
                Debug.Log("This is the sds of the object " + selectorScripts[i].gameObject.GetComponent<ObjectContents>().currentContent);
                //Use object contents.contents to find sds files in filemanager
                //Application.OpenURL("file:///c:/filename.PDF");
                //display SDS
            }
        }
    }

}
