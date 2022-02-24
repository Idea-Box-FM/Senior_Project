using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContents : MonoBehaviour
{
    //public static ObjectContents contents;

    //This is a enum for the 9 different Hazard types
    public enum Contents { HealthHazard, Flammability, CompressedGas, Corrosive, Explosive, Oxidizers, Environmental, AcuteToxicity, Other, None};

    [Tooltip("This variable is used to set the contents of the object")]
    public Contents currentContent = Contents.None;


    /// <summary>
    /// This function is meant to change the contents of the object this script is on
    /// </summary>
    /// <param name="c">The Input for the is the index number of the Contents enum + 1 </param>
    public void ChangeContents(int c)
    {
        switch(c)
        {
            case 1:
                currentContent = Contents.HealthHazard;
                break;
            case 2:
                currentContent = Contents.Flammability;
                break;
            case 3:
                currentContent = Contents.CompressedGas;
                break;
            case 4:
                currentContent = Contents.Corrosive;
                break;
            case 5:
                currentContent = Contents.Explosive;
                break;
            case 6:
                currentContent = Contents.Oxidizers;
                break;
            case 7:
                currentContent = Contents.Environmental;
                break;
            case 8:
                currentContent = Contents.AcuteToxicity;
                break;
            case 9:
                currentContent = Contents.Other;
                break;
            case 10:
                currentContent = Contents.None;
                break;
        }

    }



}
