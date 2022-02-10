using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContents : MonoBehaviour
{
    //public static ObjectContents contents;

    //This is a enum for the 9 different Hazard types
    //public enum Contents { HealthHazard, Flammability, CompressedGas, Corrosive, Explosive, Oxidizers, Environmental, AcuteToxicity, Other, None};
    public string[] contents;

    // [Tooltip("This variable is used to set the contents of the object")]
    //public Contents currentContent = Contents.None;

    public string currentContent;



    void Start()
    {
        contents = FileManager.fileManager.sdsFiles;
        currentContent = contents[0];
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
