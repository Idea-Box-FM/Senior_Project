using UnityEngine;

public class Preview : MonoBehaviour
{
    public FMPrefab prefab;

    public bool CanPlace
    {
        get
        {
            return true;
        }
    }

    public XML ConvertToXML()
    {
        return prefab.CreateSpawnXML(this); //prefab.InstanciatePrefab(transform.position, transform.rotation).GetComponent<FMInfo>(); //TODO change return type tp return FMInfo
    }
}