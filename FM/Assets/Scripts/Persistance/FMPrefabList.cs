using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Flower Box
 *Programmer: Patrick Naatz
 *Intention: to make a class capable of storing the data needed for prefabs in a build.
 *since there is no way to technically do that without lots of editor rewrites it is
 *simpler to dedicate a class to live data storage
 *Edited by Patrick Naatz
 *  Added Singleton 1/31/2022
 */
public class FMPrefabList : MonoBehaviour
{
    public FMPrefab[] prefabs;

    public static FMPrefabList prefabList;

    private void Start()
    {
        if(prefabList == null)
        {
            prefabList = this;
        } else
        {
            Destroy(this);
        }
    }

    public FMPrefab GetPrefabType(GameObject originalObject)
    {
        foreach(FMPrefab prefab in prefabs)
        {
            if (prefab.IsThisTypeOfPrefab(originalObject))
            {
                return prefab;
            }
        }

        Debug.LogError(originalObject.name + " is not a FMPrefab");
        return null;
    }
}
