using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDSRead : MonoBehaviour
{
    public ObjectContents objectContents;

    void Start()
    {
        objectContents.contents = FileManager.fileManager.sdsFiles;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
