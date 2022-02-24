using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*Flower Box
 * 
 * Intention: Select objects in Play mode to access ability to read SDS
 * 
 * Editor: Tyler Rubenstein
 *   Added to main 2/17/21
 * 
 */

public class PlaySelector : MonoBehaviour
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
    public Button cancelButton;


    public bool isSelected = false;

    //private UnityAction action;
    //private UnityAction deselect;
    //private UnityAction delete;

    //Set materials on Awake, otherwise new objects will use the testMat
    void Awake()
    {
        //goMaterial = transform.gameObject.GetComponent<MeshRenderer>();
        //selfMat = goMaterial.material;
        if (SceneManager.GetActiveScene().name == "Game Scene")
            cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        else
        {
            return;
        }
    }

    //find the main Camera
    void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();
        //if (gameObject.tag == "FMPrefab")
        //{
        //    follower = gameObject.GetComponent<FollowScript>();
        //}

        //action = new UnityAction(MoveItem);
        //deselect = new UnityAction(Deselect);
        //delete = new UnityAction(Delete);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame /*PUT TOUCH BASED FUNCTIONALITY HERE*/)
        {
            //raycast from camera to mouse location
            Ray selectRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit selectHit;

            //If the raycast hits an object under the selectMask
            if (Physics.Raycast(selectRay, out selectHit, Mathf.Infinity, selectMask) /*&& selectHit.transform == this.transform*/)
            {
                if (selectHit.collider.tag == "FMPrefab")
                {
                    //change material of the raycasted object to the testMat
                    selectHit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    for (int i = 0; i < selectHit.transform.gameObject.transform.childCount; i++)
                    {
                        selectHit.transform.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }

                    selectHit.transform.gameObject.GetComponent<PlaySelector>().isSelected = true;

                    //if (isSelected == true)
                    //{
                    //    moveButton.onClick.AddListener(action);
                    //    cancelButton.onClick.AddListener(deselect);
                    //    deleteButton.onClick.AddListener(delete);
                    //}
                }
            }
        }


        if (Keyboard.current.spaceKey.wasPressedThisFrame && isSelected)
        {
            //change the material back to selfMat when you press space
            goMaterial.material = selfMat;
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                this.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.white;
            }

            isSelected = false;

            //follower.enabled = false;            
            //moveButton.onClick.RemoveListener(action);
            //cancelButton.onClick.RemoveListener(deselect);
            //deleteButton.onClick.RemoveListener(delete);
        }
    }



    public void Deselect()
    {
        //change the material back to selfMat when you click off of an object
        goMaterial.material = selfMat;
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.white;
        }

        isSelected = false;
        //follower.enabled = false;
        //moveButton.onClick.RemoveListener(action);
        //cancelButton.onClick.RemoveListener(deselect);
        //deleteButton.onClick.RemoveListener(delete);
    }
}
