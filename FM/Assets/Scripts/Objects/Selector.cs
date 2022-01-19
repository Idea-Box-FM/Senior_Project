using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Flower Box
 * 
 * Intention: When you hover over an object, it will change it's material
 * 
 * Editor: Tyler Rubenstein
 *   Added to main 12/7/21
 */

public class Selector : MonoBehaviour
{
    //layer mask to select items
    public LayerMask selectMask;
    //object's original material
    public Material selfMat;
    //material used after clicking the object
    public Material testMat;
    //main camera
    public Camera mainCamera;
    //mesh renderer for materials
    public MeshRenderer goMaterial;

    //RYAN DID THIS
    FollowScript follower;
    
    //Set materials on Awake, otherwise new objects will use the testMat
    void Awake()
    {
        goMaterial = transform.gameObject.GetComponent<MeshRenderer>();
        selfMat = goMaterial.material;
        if (gameObject.tag == "FMPrefab")
        {
            follower = gameObject.GetComponent<FollowScript>();
        }
    }

    //find the main Camera
    void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();       
    }

    // Update is called once per frame
    void Update()
    {
        //raycast from camera to mouse location
        Ray selectRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit selectHit;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //If the raycast hits an object under the selectMask
            if (Physics.Raycast(selectRay, out selectHit, Mathf.Infinity, selectMask))
            {
                //change material of the raycasted object to the testMat
                selectHit.transform.gameObject.GetComponent<MeshRenderer>().material = testMat;
                
                //again, RYAN's fault
                if (gameObject.tag == "FMPrefab")
                {
                    follower.enabled = true;
                }
            }
           else
           {
                //change the material back to selfMat when you click off of an object
                goMaterial.material = selfMat;
                
                //again, RYAN's fault
                if (gameObject.tag == "FMPrefab")
                {
                    follower.enabled = false;
                }
           }
        }  

        //more RYAN
        if(Mouse.current.rightButton.wasPressedThisFrame)
        {
            //change the material back to selfMat when you click off of an object
            goMaterial.material = selfMat;

            //again, RYAN's fault
            if (gameObject.tag == "FMPrefab")
            {
                follower.enabled = false;
            }
        }

    }
}
