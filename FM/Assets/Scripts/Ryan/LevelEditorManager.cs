using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Flower Box
 * 
 * Intention: Make a manager that saves the state of the editor
 * 
 * Editor: Patrick Naatz
 *   Proper file saving 11/1/21
 *   Added Comments 11/2/21
 *   Merged EditingManager with LevelEditing Manager 11/8/2021
 *   Added properties to simplify readablility 11/8/2021
 *   Added the save functionality 11/15/2021
 *   TEMP commented out Barrell because it was not being used for anything just causing an error
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

    #region Fields

    //Barell barell;

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
    //layer mask for the raycast to change wall texture
    public LayerMask wallMask;
    //layer mask for the raycast to place items
    public LayerMask mask;   
    //grabs collision script for collision
    public CollisionDetect collision;

    public Material material1;     

    GameObject hitTarget;

    XML xml;
    FileManager fileManager;
    Room room;
    #endregion

    void Start()
    {
        prefabList = GetComponent<FMPrefabList>();
        fileManager = FileManager.fileManager;
        room = FindObjectOfType<Room>();
        //barell = GameObject.FindGameObjectWithTag("FMPrefab").GetComponent<Barell>();
    }

    private void Update()
    {
        

        //if the left mouse button is clicked and a button has been clicked, spawn a prefab at the mouse/raycast location
        if (Mouse.current.leftButton.wasPressedThisFrame && CurrentButton.isClicked)
        {
            GameObject example = GameObject.FindGameObjectWithTag("GoodPrefab");
            //ray from camera to mouse location
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            //if the raycast hits something on the layer mask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask) && collision.canPlace == true)
            {
                //set the bool back to false -- this needs to be changed to a state machine so we can place multiple items and switch items with the buttons
                CurrentButton.isClicked = false;

                //instantiate prefab based on current button pressed at the raycast hit location
                GameObject finalPrefab = CurrentPrefab.InstanciatePrefab(hit.point, Quaternion.Euler(example.transform.eulerAngles));
                finalPrefab.SetActive(true);

                DestroyCurrentExample();
            }
        }

        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            //raycast from main camera to mouse position
            Ray deleteRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit deleteHit;

            //if the raycast hits a valid target on the layer mask, destroy the object
            if (Physics.Raycast(deleteRay, out deleteHit, Mathf.Infinity, deleteMask))
            {
                //hightlights game object by parent -- IMPORTANT -- ALL ITEMS NEED TO HAVE A PARENT
                hitTarget = deleteHit.transform.parent.gameObject;
            }
        }

        //if the middle mouse button is clicked, ray cast out
        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            //raycast from main camera to mouse position
            Ray deleteRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit deleteHit;

            //if the raycast hits a valid target on the layer mask, destroy the object
            if(Physics.Raycast(deleteRay, out deleteHit, Mathf.Infinity, deleteMask))
            {
                //destroys game object by parent -- IMPORTANT -- ALL ITEMS NEED TO HAVE A PARENT
                Destroy(deleteHit.transform.parent.gameObject);
            }
        }

        //if m is pressed, change material on wall only
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            //raycast from main camera to mouse position
            Ray wallRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit wallHit;

            //if the raycast hits a valid target on the wall layer mask, change the material
            if (Physics.Raycast(wallRay, out wallHit, Mathf.Infinity, wallMask))
            {
                wallHit.transform.gameObject.GetComponent<MeshRenderer>().material = material1;
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
        room.ConvertToXML(ref xml);
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

        xml.ExportXML(fileManager.currentSimulation);
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
