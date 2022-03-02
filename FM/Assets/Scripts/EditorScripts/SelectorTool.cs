using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*FLowerBox
 * Programmer: Patrick Naatz
 * Intention: an attempt at redoing the seelction script to work with the play and editor scene
 * How: This tool will be used to store all the selected objects in one persistant scripts and give functions that can be used in other script where selection is needed
 */
public class SelectorTool : MonoBehaviour
{
    public static SelectorTool selectorTool;

    [SerializeField] List<FMInfo> selectedObjects; //remove serialize field here
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask selectMask;
    [SerializeField] int maxSelectedObjects = int.MaxValue;//remove serialize field here //this value is set by default to editor value

    Transform groupingTool;

    Button[] relevantButtons;


    Button cancelButton;
    Button CancelButton
    {
        get
        {
            if (cancelButton == null)
            {
                foreach (Button button in relevantButtons)
                {
                    if (button.name == "CancelButton")
                    {
                        cancelButton = button;
                        break;
                    }
                }
            }

            return cancelButton;
        }
    }

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        Singleton();

        mainCamera = GameObject.FindObjectOfType<Camera>();
        relevantButtons = GameObject.Find("ButtonList").GetComponentsInChildren<Button>();
        groupingTool = GameObject.Find("Group").GetComponent<Transform>();

        if(SceneManager.GetActiveScene().name == "Game Scene")
        {
            maxSelectedObjects = 1;
        }

        CancelButton.onClick.AddListener(Cancel);
    }

    private void Singleton()
    {
        if(selectorTool == null)
        {
            selectorTool = this;
        } else
        {
            Destroy(this);
        }
    }
    #endregion

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //raycast from camera to mouse location
            Ray selectRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit selectHit;

            //If the raycast hits an object under the selectMask
            if (Physics.Raycast(selectRay, out selectHit, Mathf.Infinity, selectMask))
            {
                //Debug.Log("Selection tool hit " + selectHit.transform.name);
                if (selectHit.collider.tag == "FMPrefab")
                {
                    //Debug.Log("It is an fmprefab");
                    Transform selectedObject = selectHit.transform;
                    if (IsSelected(selectedObject))
                    {
                        Deselect(selectedObject);
                    } else
                    {
                        Select(selectedObject);
                    }
                } else
                {
                    Debug.Log("It is not an fmprefab");
                }
            }
        }
    }

    #region (De)Select Functionality
    public void Select(Component component)
    {
        FMInfo info = LevelEditorManager.SearchForObjectType<FMInfo, FMInfo>(component.transform);

        if (IsSelected(info)) //Gaurd statement
        {
            Debug.LogError(info.name + " is already selected");
            return;
        }

        selectedObjects.Add(info);

        CreateSelectedObject(info);
        info.gameObject.SetActive(false);

        if (selectedObjects.Count > maxSelectedObjects)
        {
            Deselect(selectedObjects[0]);
        }
        else //else just to skip calling the updateButtons function twice
        {
            UpdateButtons();
        }
    }

    public void Deselect(Component component)
    {
        FMInfo info = LevelEditorManager.SearchForObjectType<FMInfo, FMInfo>(component.transform);

        if (!IsSelected(info)) //Gaurd statement
        {
            Debug.LogError("Cant deselect " + info.name + " because it is not already selected");
            return;
        }

        selectedObjects.Remove(info);
        DestroySelectedObject(info.GetComponent<FMInfo>());
        info.gameObject.SetActive(true);
        UpdateButtons();

    }

    #region Selected Object Functions
    private void CreateSelectedObject(FMInfo selectedInfo)
    {
        if (selectedInfo.selectedObject != null) //Gaurd Statement
        {
            Debug.LogError("You are making to selected objects of " + selectedInfo.name);
            return;
        }

        selectedInfo.selectedObject = Instantiate(selectedInfo.basePrefab.selectorPrefab, selectedInfo.transform.position, selectedInfo.transform.rotation, null);
    }

    private void DestroySelectedObject(FMInfo selectedInfo)
    {
        if(selectedInfo.selectedObject == null) //gaurd statement
        {
            Debug.LogError("Trying to destroy selected object that does not exist on " + selectedInfo.name);
            return;
        }

        Destroy(selectedInfo.selectedObject);
    }
    #endregion
    
    #endregion

    public bool IsSelected(Transform transform)
    {
        FMInfo info = LevelEditorManager.SearchForObjectType<FMInfo>(transform);
        return IsSelected(info); //delegation
    }

    public bool IsSelected(FMInfo info)
    {
        return selectedObjects.Contains(info);
    }

    #region Button functionality
    private void UpdateButtons()
    {
        if(selectedObjects.Count == 0)
        {
            //Disable all functionality buttons
            foreach(Button button in relevantButtons)
            {
                button.interactable = false;
            }
        } else
        {
            //enable all functionality buttons
            foreach(Button button in relevantButtons)
            {
                button.interactable = true;
            }
        }
    }

    #region button functionality
    /// <summary>
    /// This function will unselect everything
    /// Note: Do not add this function to the cancel button, it will auto add at start
    /// </summary>
    public void Cancel()
    {
        Debug.Log("Canceling");
        while (selectedObjects.Count > 0)
        {
            Deselect(selectedObjects[0]);
        }
    }
    #endregion

    #endregion
}
