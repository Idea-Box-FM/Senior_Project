using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Flower Box
 *Programmer: Patrick Naatz
 *Intention: to make a class capable of storing the data needed for prefabs in a build.
 *since there is no way to technically do that without lots of editor rewrites it is
 *simpler to dedicate a class to live data storage
 */
public class FMPrefabList : MonoBehaviour
{
    public FMPrefab[] prefabs;
}
