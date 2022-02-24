using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContents : MonoBehaviour
{
    //Feel free to work your magic Pat

    public string[] contents;

    public string currentContent;



    void Start()
    {
        contents = FileManager.fileManager.sdsFiles;
        if (currentContent == "")
        {
            currentContent = contents[0];
        }
    }

    void Update()
    {
        
    }


    /// <summary>
    /// This function is meant to change the contents of the object this script is on
    /// </summary>
    /// <param name="c">The Input for the is the index number of the Contents enum + 1 </param>
    public void ChangeContents(int c)
    {

        currentContent = contents[c];

    }



}
