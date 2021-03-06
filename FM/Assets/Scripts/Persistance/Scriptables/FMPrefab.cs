using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*Flower Box
 * Programmer: Patrick Naatz
 * Intention: Make an object capable of running as prefabs in the build version of the game
 * How to use: inherit classes from FMPrefab of specific types of prefab, override the Instanciate Prefab functions and ConvertToXML function to save specifics about each item
 * Remember To: tag the prefab as FMPrefab
 * 
 * TODO modify for scale
 * 
 * Editited:
 *  Made it so the Instanciation automatically implements the changes in spacing 11/8/2021
 *
 * Edited (Dylan):
 *  Added Audio sound effect requirements 2/9/2022
 */

[CreateAssetMenu(fileName = "FMPrefab", menuName = "FMPrefabs/FMPrefab")]
public class FMPrefab : ScriptableObject
{
    [Tooltip("Make sure this prefab is tagged FMPrefab")]
    [SerializeField] GameObject prefab;
    public GameObject examplePrefab;
    public GameObject selectorPrefab;
    public float height;

    [HideInInspector]
    public GameObject parent;

    public GameObject soundEffect;

    #region InstanciatePrefab

    public virtual GameObject InstanciateExample()
    {
        //instantiate the example item(green) based on the id of the clicked button
        return Instantiate(examplePrefab, new Vector3(0, 2, 0), Quaternion.identity);
    }

    public virtual GameObject InstanciatePrefab(XML xml)
    {
        Vector3 position = ConvertToVector3(xml.attributes["Position"]);
        Quaternion rotation = ConvertToQuaternion(xml.attributes["Rotation"]);

        return InstanciatePrefab(position, rotation, xml.name);
    }

    public GameObject InstanciatePrefab(Vector3 position, Quaternion? Rotation = null, string name = "")
    {
        if (parent == null)
        {
            parent = new GameObject();
            parent.name = prefab.name;
        }

        Rotation ??= Quaternion.identity;

        GameObject.Find("EffectPlayer").GetComponent<PlaySoundEffect>().Play(1);//play placement sound here//Very innefficent due to needing an instance that exists only in the scene (im sorry Pat)

        GameObject instance = Instantiate(prefab, position + examplePrefab.transform.position, Rotation.Value, parent: parent.transform);
        instance.SetActive(false);

        if (name != "")
        {
            instance.name = name;
        }

        return instance;
    }
    #endregion

    #region Helper Functions
    public static Vector3 ConvertToVector3(string line)
    {
        line = line.Substring(1, line.Length - 2);
        line = line.Replace(" ", "");
        List<string> sections = line.Split(',').ToList();

        float x = float.Parse(sections[0]);
        float y = float.Parse(sections[1]);
        float z = float.Parse(sections[2]);

        return new Vector3(x,y,z);
    }

    public static Quaternion ConvertToQuaternion(string line)
    {
        line = line.Substring(1, line.Length - 2);
        line = line.Replace(" ", "");
        List<string> sections = line.Split(',').ToList();

        float x = float.Parse(sections[0]);
        float y = float.Parse(sections[1]);
        float z = float.Parse(sections[2]);
        float w = float.Parse(sections[3]);

        return new Quaternion(x, y, z, w);
    }
    #endregion

    /// <summary>
    /// When overiding this function it is recommended you first call the base function then edit the xml returned. However not required
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public virtual XML ConvertToXML(GameObject gameObject)
    {
        if ((!IsFMPrefab(gameObject) || !IsThisTypeOfPrefab(gameObject)) && !IsPreview(gameObject))
        {
            return null;
        }
        
        XML child = new XML(gameObject.name.Replace(" ", ""));

        //NOTE we use local transforms because we will be reparenting it to a parent
        //Note we remove the original prefabs location because we systematically add it in the instanciate function
        child.attributes.Add("Position", (gameObject.transform.localPosition - examplePrefab.transform.position).ToString());
        child.attributes.Add("Rotation", gameObject.transform.localRotation.ToString());

        return child;
    }

    #region Logic Functions
    static bool IsFMPrefab(GameObject gameObject)
    {
        return gameObject.tag == "FMPrefab";
    }

    public bool IsThisTypeOfPrefab(GameObject gameObject)
    {
        return gameObject.name.Contains(prefab.name);
    }

    bool IsPreview(GameObject gameObject)
    {
        return gameObject.GetComponent<Preview>() != null;
    }

    public XML CreateSpawnXML(Preview preview)
    {
        Transform parent = preview.transform.parent;
        preview.transform.parent = null;

        XML xml = ConvertToXML(preview.gameObject);
        xml.name = this.parent.name + "(Clone)";

        preview.transform.parent = parent;

        Debug.Log("Spawn XML " + xml.ToLine());

        return xml;
    }
    #endregion
}
