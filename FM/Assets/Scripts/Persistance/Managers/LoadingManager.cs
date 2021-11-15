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
 */

[RequireComponent(typeof(FMPrefabList))]
public class LoadingManager : MonoBehaviour
{
    FMPrefabList prefabList;
    FileManager fileManager;

    // Start is called before the first frame update
    void Start()
    {
        prefabList = GetComponent<FMPrefabList>();
        fileManager = FileManager.fileManager;

        XML xml = XML.readfromfile(fileManager.currentSimulation);

        //loads the room size
        FindObjectOfType<RoomLoader>().RoomSize = FMPrefab.ConvertToVector3(xml.attributes["RoomSize"]); ;
            
        LoadFromXML(xml);
    }

    void LoadFromXML(XML xml)
    {
        List<GameObject> newObjects = new List<GameObject>(); //we save the gameobjects, disabled to this list because they have rigidbodies, we do not want gravity taking effect while we are still spawning the parts

        foreach (FMPrefab prefab in prefabList.prefabs)
        {
            XML section;
            if ((section = xml.FindChild(prefab.name)) != null)
            {
                foreach (XML subObject in section.subObjects)
                {
                    GameObject gameObject = prefab.InstanciatePrefab(subObject);
                    newObjects.Add(gameObject);
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
        }
    }
}
