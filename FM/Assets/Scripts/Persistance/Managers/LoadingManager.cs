using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*Flower Box
 * Programmer: Patrick Naatz
 * Intention: Make a script capable of loadining in the data from the saved XML file
 * TODO: Make it so the player chooses which simulation to load
 * 
 * Edited by: Pat Naatz
 *  proper file loading 11/1/21
 *  Add comments 11/2/21
 *  Added the room loading 11/15/2021
 *  Removed some debugging features 11/27/2021
 *  Changed FMPrefabList to singleton pattern 2/2/2022
 */

public class LoadingManager : MonoBehaviour
{
    FileManager fileManager;

    // Start is called before the first frame update
    void Start()
    {
        fileManager = FileManager.fileManager;

        XML xml = XML.readfromfile(fileManager.currentSimulation);
 
        //loads the room
        FindObjectOfType<Room>().LoadFromXML(xml);
            
        LoadFromXML(xml);
    }

    void LoadFromXML(XML xml)
    {
        List<GameObject> newObjects = new List<GameObject>(); //we save the gameobjects, disabled to this list because they have rigidbodies, we do not want gravity taking effect while we are still spawning the parts

        foreach (FMPrefab prefab in FMPrefabList.Prefabs)
        {
            XML section;
            if ((section = xml.FindChild(prefab.name)) != null)
            {
                foreach (XML subObject in section.subObjects)
                {
                    GameObject gameObject = prefab.InstanciatePrefab(subObject);
                    newObjects.Add(gameObject);
                    if (section.name == "IBC")
                    {
                        //Debug.LogError("Loading IBC");
                        Debug.Log(gameObject.name);
                        Transform ibcBase = gameObject.transform.FindChild("IBCBase");
                        ObjectContents oC = ibcBase.GetComponentInChildren<ObjectContents>();
                        Debug.Log(oC.currentContent);
                        oC.currentContent = subObject.attributes["SDS"];
                        Debug.Log(oC.currentContent);
                    }
                }

                
            }
        }

        ActivateParts(newObjects);
    }

    private static void ActivateParts(List<GameObject> newObjects)
    {
        foreach (GameObject gameObject in newObjects)
        {
            gameObject.SetActive(true);
            if(gameObject.name == "IBC(Clone)")
            {
                Debug.Log("found it activating");
                Debug.Log(gameObject.GetComponentInChildren<ObjectContents>().currentContent);
            }
        }
    }
}
