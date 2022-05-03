using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SelectionTool : Monkey
{
    public static SelectionTool selectionTool;

    //selected objects
    [SerializeField] List<FMInfo> selectedObjects = new List<FMInfo>();
    int maxSelectedObjects = int.MaxValue;//this value is set by default to editor value

    //Raycast variables
    Camera mainCamera;
    [SerializeField] LayerMask objectMask;

    public static FMInfo[] SelectedObjects
    {
        get
        {
            return selectionTool.selectedObjects.ToArray();
        }
    }

    private void Awake()
    {
        SingletonPattern<SelectionTool>(ref selectionTool, this);
    }

    private void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();

        if (SceneManager.GetActiveScene().name == "Game Scene")
        {
            maxSelectedObjects = 1;
        }
    }

    #region MakeCommand
    public override Command MakeCommand(params object[] parameters)
    {
        FMInfo info = parameters[0] as FMInfo;

        return MakeCommand(info);
    }

    public Command MakeCommand(FMInfo info)
    {
        Command command = null;
        Transform selectedObject = info.transform;
        if (IsSelected(selectedObject))
        {
            command = new DeselectCommand(info);
            //Deselect(selectedObject);
        }
        else
        {
            if (selectedObjects.Count + 1 > maxSelectedObjects)
            {
                Command deselectCommand = new DeselectCommand(selectedObjects[selectedObjects.Count - 1]);
                Command selectCommand = new SelectCommand(info);
                command = new MultiStepCommand(deselectCommand, selectCommand);
            }
            else
            {
                command = new SelectCommand(info);
            }

            levelEditingManager.Execute(command);
        }   

        return command;
    }
    #endregion

    private void Mouse_Click(InputAction.CallbackContext obj)
    {
        //raycast from camera to mouse location
        Ray selectRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit selectHit;

        //If the raycast hits an object under the selectMask
        if (Physics.Raycast(selectRay, out selectHit, Mathf.Infinity, objectMask))
        {
            if (selectHit.collider.tag == "FMPrefab")
            {
                FMInfo info = selectHit.transform.GetComponent<FMInfo>();
                Command command = MakeCommand(info);
                levelEditingManager.Execute(command);
            }
            else
            {
                DeselectAll();
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
        if (selectedInfo.selectedObject == null) //gaurd statement
        {
            Debug.LogError("Trying to destroy selected object that does not exist on " + selectedInfo.name);
            return;
        }

        Destroy(selectedInfo.selectedObject);
    }

    public void DeselectAll()
    {
        MultiStepCommand command;

        List<DeselectCommand> commands = new List<DeselectCommand>();

        foreach(FMInfo info in selectedObjects)
        {
            DeselectCommand deselectCommand = new DeselectCommand(info);
            commands.Add(deselectCommand);
        }

        command = new MultiStepCommand(commands.ToArray());
        levelEditingManager.Execute(command);
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
}
