using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMInfo : MonoBehaviour
{
    public FMPrefab basePrefab;
    public GameObject selectedObject;
    public bool IsSelected
    {
        get
        {
            return SelectorTool.selectorTool.IsSelected(transform);
        }

        set
        {
            if (value)
            {
                SelectorTool.selectorTool.Select(transform);
            } else
            {
                SelectorTool.selectorTool.Deselect(transform);
            }
        }
    }
}
