using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*FLOWERBOX
 *Programmer: Patrick Naatz
 *Intention: Make a class capable of carrying the temporary info of all objects currently copied
 *TODO: consider actually moving this to the clipboard--Note there is a issue when using ctrl+c while playing in editor because unity also knows what ctrl+c means
 */
public class CopyInfo
{
    #region fields
    XML details;
    FMPrefab originalPrefab;
    Vector3 offset;

    Transform example;

    public static Transform group;
    #endregion

    public Vector3 CenterPoint
    {
        set
        {
            offset = FMPrefab.ConvertToVector3(details.attributes["Position"]) - value;
        }
    }

    public CopyInfo(FMPrefab originalPrefab, XML details)
    {
        this.originalPrefab = originalPrefab;
        this.details = details;
    }

    #region Behaviors
    public GameObject Preview()
    {
        example = originalPrefab.InstanciateExample().transform;
        example.parent = group.transform;
        example.GetChild(0).transform.localPosition = offset;//NOTE for some reason example.transform.localPosition/Position does not actually change the location of the object. Unity Glitch

        return example.gameObject;
    }

    public GameObject Instanciate()
    {
        GameObject newObject = originalPrefab.InstanciatePrefab(details);
        newObject.transform.position = example.GetChild(0).transform.position;//See Preview for explanation
        newObject.SetActive(true);

        return newObject;
    }
    #endregion
}
