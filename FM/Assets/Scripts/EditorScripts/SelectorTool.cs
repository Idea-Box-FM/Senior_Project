using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/*FLowerBox
 * Programmer: Patrick Naatz
 * Intention: an attempt at redoing the seelction script to work with the play and editor scene
 * How: This tool will be used to store all the selected objects in one persistant scripts and give functions that can be used in other script where selection is needed
 * Usage: you can access the selected objects through static data/behaviors however selection functionality requires you to get the instance of the selector tool
 */
public class SelectorTool : MonoBehaviour
{
    #region Fields
    public static SelectorTool selectorTool;

    //selected objects
    List<FMInfo> selectedObjects = new List<FMInfo>();
    int maxSelectedObjects = int.MaxValue;//this value is set by default to editor value

    //Raycast variables
    Camera mainCamera;
    [SerializeField] LayerMask objectMask;

    //controls
    Button[] relevantButtons;
    CameraControl cameraControl;
    #endregion

    #region Properties
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

    public static FMInfo[] SelectedObjects
    {
        get
        {
            return selectorTool.selectedObjects.ToArray();
        }
    }
    #endregion

    #region Start
    private void Awake()
    {
        cameraControl = new CameraControl();
        cameraControl.Editor.Click.canceled += Mouse_Click;
    }

    // Start is called before the first frame update
    void Start()
    {
        Singleton();

        mainCamera = GameObject.FindObjectOfType<Camera>();
        relevantButtons = GameObject.Find("ButtonList").GetComponentsInChildren<Button>();

        if(SceneManager.GetActiveScene().name == "Game Scene")
        {
            maxSelectedObjects = 1;
        }

        CancelButton.onClick.AddListener(Cancel);

    }

    private void OnEnable()
    {
        cameraControl.Enable();//enable every action map
    }

    private void OnDisable()
    {
        cameraControl.Disable();//disable every action map
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

    private void Mouse_Click(InputAction.CallbackContext obj)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {//If not clicking on UI
            //raycast from camera to mouse location
            Ray selectRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit selectHit;

            //If the raycast hits an object under the selectMask
            if (Physics.Raycast(selectRay, out selectHit, Mathf.Infinity, objectMask))
            {
                if (selectHit.collider.tag == "FMPrefab")
                {
                    Transform selectedObject = selectHit.transform;
                    if (IsSelected(selectedObject))
                    {
                        Deselect(selectedObject);
                    }
                    else
                    {
                        Select(selectedObject);
                    }
                } else
                {
                    DeselectAll();
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

    public void DeselectAll()
    {
        while(selectedObjects.Count > 0)
        {
            Deselect(selectedObjects[0]);
        }
    }
    #endregion

    #endregion

    #region IsSelected
    public bool IsSelected(Transform transform)
    {
        FMInfo info = LevelEditorManager.SearchForObjectType<FMInfo>(transform);
        return IsSelected(info); //delegation
    }

    public bool IsSelected(FMInfo info)
    {
        return selectedObjects.Contains(info);
    }
    #endregion

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
