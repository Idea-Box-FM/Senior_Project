using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelEditorManager : MonoBehaviour
{
    //Ryan Consentino

    //array of buttons for the different items
    public ItemController[] itemButtons;
    //array of prefabs for the different items
    public GameObject[] itemPrefabs;
    //array of prefabs for the example(green/good) items
    public GameObject[] itemExample;
    //reference to the current button that is clicked to spawn the appropriate item
    public int currentButtonPressed;
    //reference to the main camera
    public Camera mainCamera;
    //layer mask for the raycast to delete items
    public LayerMask deleteMask;
    //layer mask for the raycast to place items
    public LayerMask mask;

    public CollisionDetect collision;

    private void Start()
    {
        collision = GameObject.FindGameObjectWithTag("Wall").GetComponent<CollisionDetect>();
    }

    private void Update()
    {
        //if the left mouse button is clicked and a button has been clicked, spawn a prefab at the mouse/raycast location
        if (Mouse.current.leftButton.wasPressedThisFrame && itemButtons[currentButtonPressed].isClicked)
        {
            //set the bool back to false -- this needs to be changed to a state machine so we can place multiple items and switch items with the buttons
            itemButtons[currentButtonPressed].isClicked = false;
            //ray from camera to mouse location
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            //if the raycast hits something on the layer mask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                //instantiate prefab based on current button pressed at the raycast hit location
                Instantiate(itemPrefabs[currentButtonPressed], new Vector3(hit.point.x, hit.point.y + itemPrefabs[currentButtonPressed].transform.position.y, hit.point.z),
                    Quaternion.identity);

                //destroy the example prefab(green prefab)
                Destroy(GameObject.FindGameObjectWithTag("GoodPrefab"));
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
                //destroys parent of game object -- WORKS FOR SHELF RIGHT NOW BUT NOT FOR BARREL
                Destroy(deleteHit.transform.gameObject);
            }
        }
    }
}
