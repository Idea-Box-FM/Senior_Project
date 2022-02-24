using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*FlowerBox
 * Programmer: Patrick Naatz
 * Intention: Make a script ot go on each object that requires SDS data
 * 
 * Edited:
 * Merged the functionality of ObjectContents with SDSInfo 2/24/2022
 * Changed some naming conventions in SDSInfo to match ObjectContents 2/24/2022
 */
public class SDSInfo : MonoBehaviour
{
    #region Fields
    public string currentContent;

    [SerializeField][Range(0,4)] int flamability, reactivity, health;
    #endregion

    #region Properties
    public int Flamability
    {
        get
        {
            return flamability;
        }

        set
        {
            flamability = Mathf.Clamp(value, 0, 4);
        }
    }
    public int Reactivity
    {
        get
        {
            return reactivity;
        }

        set
        {
            reactivity = Mathf.Clamp(value, 0, 4);
        }
    }
    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = Mathf.Clamp(value, 0, 4);
        }
    }

    public string[] Contents
    {
        get
        {
            return FileManager.fileManager.sdsFiles;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (currentContent == "")
        {
            currentContent = Contents[0];
        }
    }

    /// <summary>
    /// This function is meant to change the contents of the object this script is on
    /// </summary>
    /// <param name="c">The Input for the is the index number of the Contents enum + 1 </param>
    public void ChangeContents(int c)
    {
        currentContent = Contents[c];
    }
}
