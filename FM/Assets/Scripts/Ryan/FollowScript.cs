using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Editor: Patrick Naatz
 *  fixed variable names that no longer existed in LevelEditingManager 11/8/2021
 */  

public class FollowScript : MonoBehaviour
{
    //Ryan Consentino

    //reference to the editor manager itself
    private LevelEditorManager editor;
    //reference to the camera
    private Camera mainCamera;
    //layer mask for raycasts to determine what the raycast should interact with or not
    public LayerMask mask;

    void Start()
    {
        //grab the LevelEditorManager component
        editor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelEditorManager>();
        //grab the main camera
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //make a ray from the camera to the mouse position on screen
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        //hit point for the raycast
        RaycastHit hit;

        //if the raycast is hitting a valid object within the layer mask, have the instantiated prefab from the editor(in this case its the example prefab) follow the mouse
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            //move prefab position to where the raycast is hitting on the x, y and z. For the y, add the prefabs y transform so that it is appropriately above the ground

            transform.position = new Vector3(hit.point.x, hit.point.y + editor.CurrentPrefab.examplePrefab.transform.position.y, hit.point.z);

            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                transform.eulerAngles += new Vector3(0, 90, 0);
            }
            if (Keyboard.current.vKey.wasPressedThisFrame)
            {
                transform.eulerAngles += new Vector3(0, 0, 90);
            }
        }
    }
}
