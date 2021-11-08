using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Flower Box
 * 
 * Intention: Make a manager that saves the state of the editor
 * 
 * Editor: Patrick Naatz
 *   Merged EditingManager with LevelEditing Manager 11/8/2021
 *   Added properties to simplify readablility 11/8/2021
 */

[RequireComponent(typeof(FMPrefabList))]
public class LevelEditorManager : MonoBehaviour
{
    //Ryan Consentino

    #region Properties
    public FMPrefab CurrentPrefab
    {
        get
        {
            return prefabList.prefabs[currentButtonPressed];
        }
    }

    ItemController CurrentButton
    {
        get
        {
            return itemButtons[currentButtonPressed];
        }
    }
    #endregion

    //array of prefabs
    FMPrefabList prefabList;

    //array of buttons for the different items
    public ItemController[] itemButtons;

    //reference to the current button that is clicked to spawn the appropriate item
    public int currentButtonPressed;
    //reference to the main camera
    public Camera mainCamera;
    //layer mask for the raycast to delete items
    public LayerMask deleteMask;
    //layer mask for the raycast to place items
    public LayerMask mask;

    XML xml;

    void Start()
    {
        prefabList = GetComponent<FMPrefabList>();
    }

    private void Update()
    {
        //if the left mouse button is clicked and a button has been clicked, spawn a prefab at the mouse/raycast location
        if(Mouse.current.leftButton.wasPressedThisFrame && CurrentButton.isClicked)
        {
            //set the bool back to false -- this needs to be changed to a state machine so we can place multiple items and switch items with the buttons
            CurrentButton.isClicked = false;
            //ray from camera to mouse location
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            //if the raycast hits something on the layer mask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                //instantiate prefab based on current button pressed at the raycast hit location
                GameObject finalPrefab = CurrentPrefab.InstanciatePrefab(hit.point, Quaternion.identity);
                finalPrefab.SetActive(true);

                DestroyCurrentExample();
            }
        }

        //if the middle mouse button is clicked, ray cast out
        if(Mouse.current.middleButton.wasPressedThisFrame)
        {
            //raycast from main camera to mouse position
            Ray deleteRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit deleteHit;

            //if the raycast hits a valid target on the layer mask, destroy the object
            if(Physics.Raycast(deleteRay, out deleteHit, Mathf.Infinity, deleteMask))
            {
                //destroys parent of game object -- WORKS FOR SHELF RIGHT NOW BUT NOT FOR BARREL
                Destroy(deleteHit.transform.gameObject);
            }
        }
    }

    public static void DestroyCurrentExample()
    {
        //destroy the example prefab(green prefab)
        GameObject example = GameObject.FindGameObjectWithTag("GoodPrefab");
        if (example != null)
        {
            Destroy(example);
        }
    }

    /// <summary>
    /// call this function when you want to make a save of the scene
    /// </summary>
    public void Save() //NOTE currently this function will override the existing file without prompting
    {
        xml = new XML();
        foreach (FMPrefab prefab in prefabList.prefabs)
        {
            if (prefab.parent == null)
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

        xml.ExportXML("levelX.XML");
    }

    /// <summary>
    /// Takes the child elements and converts it to XML tags
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="section"></param>
    /// <param name="prefab"></param>
    /// <returns>returns whether this xml is worth saving or not</returns>
    bool ConvertChildrenToXML(ref XML parent, Transform section, FMPrefab prefab)
    {
        bool worthSaving = false;

        foreach (Transform child in section.transform)
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
