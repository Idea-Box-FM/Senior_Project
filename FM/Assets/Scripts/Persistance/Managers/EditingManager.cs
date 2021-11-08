using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Flower Box
 * Programmer: Patrick Naatz
 * Intention: Make a manager that saves the state of the editor
 * TODO make it so the player picks which file to save to
 * 
 * Edited By: Patrick Naatz
 *  Proper file saving 11/1/21
 *  Added Comments 11/2/21
 */ 

[RequireComponent(typeof(FMPrefabList))]
public class EditingManager : MonoBehaviour
{
    #region fields
    FileManager fileManager;
    FMPrefabList prefabList;
    XML xml;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        prefabList = GetComponent<FMPrefabList>();
        fileManager = FileManager.fileManager;
    }

    /// <summary>
    /// call this function when you want to make a save of the scene
    /// </summary>
    public void Save() //NOTE currently this function will override the existing file without prompting
    {
        xml = new XML();
        foreach (FMPrefab prefab in prefabList.prefabs)
        {
            if(prefab.parent == null)
            {
                continue;
            }

            XML Section = xml.AddChild(prefab.parent.name);
            bool worthSaving = ConvertChildrenToXML(ref Section, prefab.parent.transform, prefab); //Note XML is worth saving only if it has a object inside it
            
            if (!worthSaving)
            {
                xml.RemoveChild(Section);
            }
        }

        xml.ExportXML(fileManager.currentFile);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="section"></param>
    /// <param name="prefab"></param>
    /// <returns>returns whether this xml is worth saving or not</returns>
    bool ConvertChildrenToXML(ref XML parent, Transform section, FMPrefab prefab)
    {
        bool worthSaving = false;

        foreach(Transform child in section.transform)
        {
            XML xmlChild = prefab.ConvertToXML(child.gameObject);

            if (xmlChild != null)
            {
                parent.AddChild(xmlChild);
                worthSaving = true;
            }
        }

        return worthSaving;
    }
}
