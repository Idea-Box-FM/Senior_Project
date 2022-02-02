using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*Flower Box
 * 
 * Intention: When you hover over an object, it will change it's material
 * 
 * Editor: Tyler Rubenstein
 *   Added to main 12/7/21
 * Editor: Patrick Naatz
 *  Added IsSelected Property 1/29/2022
 *  Added Deselect method 1/31/2022
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
    public Button moveButton;
    public Button cancelButton;
    public Button deleteButton;
    public bool isSelected = false;

    private UnityAction action;
    private UnityAction deselect;
    private UnityAction delete;
    
    public bool IsSelected
    {
        get
        {
            return goMaterial.material != selfMat; //for some reason goMaterial.material == testMat does not work, probably because the layers of materials used
        }
    }

    //Set materials on Awake, otherwise new objects will use the testMat
    void Awake()
    {
        goMaterial = transform.gameObject.GetComponent<MeshRenderer>();
        selfMat = goMaterial.material;
        moveButton = GameObject.Find("MoveButton").GetComponent<Button>();
        cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        deleteButton = GameObject.Find("DeleteButton").GetComponent<Button>();
    }

    //find the main Camera
    void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();
        if (gameObject.tag == "FMPrefab")
        {
            follower = gameObject.GetComponent<FollowScript>();
        }

        action = new UnityAction(MoveItem);
        deselect = new UnityAction(Deselect);
        delete = new UnityAction(Delete);
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
                selectHit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                for (int i = 0; i < selectHit.transform.gameObject.transform.childCount; i++)
                {
                    selectHit.transform.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.yellow;
                }

                selectHit.transform.gameObject.GetComponent<Selector>().isSelected = true;

                if (isSelected == true)
                {
                    moveButton.onClick.AddListener(action);
                    cancelButton.onClick.AddListener(deselect);
                    deleteButton.onClick.AddListener(delete);
                }
            }
        }  

        //more RYAN
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //change the material back to selfMat when you click off of an object
            goMaterial.material = selfMat;
            this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                this.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.white;
            }

            follower.enabled = false;
            isSelected = false;
            moveButton.onClick.RemoveListener(action);
            cancelButton.onClick.RemoveListener(deselect);
            deleteButton.onClick.RemoveListener(delete);
        }
    }

    void MoveItem()
    {
            follower.enabled = true;
    }

    void Deselect()
    {
        //change the material back to selfMat when you click off of an object
        goMaterial.material = selfMat;
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.white;
        }

        isSelected = false;
        follower.enabled = false;
        moveButton.onClick.RemoveListener(action);
        cancelButton.onClick.RemoveListener(deselect);
        deleteButton.onClick.RemoveListener(delete);
    }

    void Delete()
    {
        Destroy(this.transform.parent.gameObject);
        deleteButton.onClick.RemoveListener(delete);
    }

    public void Deselect()
    {
        goMaterial.material = selfMat;
    }
}
