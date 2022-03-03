using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*FlowerBox
 * Programmed by: Patrick Naatz
 * Intention: This Object should be on every FMPrefab to help organize data and functions used in different tools
 * Catch: However this is so late in the project that we do not use it very much
 */
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
