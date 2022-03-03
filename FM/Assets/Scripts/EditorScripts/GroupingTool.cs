using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/*FLOWERBOX
 * Programmer: Patrick Naatz
 * Purpose: make a script capable on handling copying and pasting of multiple objects
 * TODO remove debugging pieces cignified by //Debugging
 * Please test persistance, load scene use copy, exit simulation and relaunch, ensuring copy and paste still works
 * 
 * Edited: Pat Naatz
 *  Changed FMPrefabList to singleton pattern 2/2/2022
 *  Updated to work with new SelectorTool 3/3/2022
 */

public class GroupingTool : MonoBehaviour
{
    #region Fields
    FMPrefabList prefabList;

    List<CopyInfo> copiedObjects = new List<CopyInfo>();
    Vector3 centerPoint = Vector3.positiveInfinity;

    CameraControl cameraControl;
    bool control = false;

    [SerializeField] Transform groupingArea;
    [SerializeField] GameObject centerPointObject; //debugging

    public enum State
    {
        Copy,
        PreviewPaste,
        Paste,
        Place
    } public State currentState = State.Copy;
    #endregion

    #region Unity functions
    private void Awake()
    {
        //Controlls
        cameraControl = new CameraControl();

        //Control
        cameraControl.Editor.Control.performed += Control_pressed;
        cameraControl.Editor.Control.canceled += Control_released;

        cameraControl.Editor.Copy.canceled += Copy_hotkey_attempted;//NOTE we add these function to the canceled function because we want to wait for them to release the command first

        //Paste
        cameraControl.Editor.Paste.canceled += hotkey_paste_attempted;
        cameraControl.Editor.Click.canceled += Mouse_Click;

        prefabList = FMPrefabList.prefabList;
    }

    private void OnEnable()
    {
        cameraControl.Enable();//enable every action map
    }

    private void OnDisable()
    {
        cameraControl.Disable();//disable every action map
    }

    public void Start()
    {
        prefabList = FMPrefabList.prefabList;
        CopyInfo.group = groupingArea;
    }
    #endregion

    #region event handling

    #region Paste
    /// <summary>
    /// Handling the ctrl + v input
    /// </summary>
    /// <param name="obj"></param>
    private void hotkey_paste_attempted(InputAction.CallbackContext obj)
    {
        if (control)
        {
            switch (currentState)
            {
                case State.Paste:
                    Paste();
                    break;
                case State.PreviewPaste:
                    Preview();
                    break;
            }
        }
    }

    /// <summary>
    /// Handling the click function when pasting
    /// </summary>
    /// <param name="obj"></param>
    private void Mouse_Click(InputAction.CallbackContext obj)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        { //if not clicking on UI
            switch (currentState)
            {
                case State.Paste:
                    Paste();
                    break;
                case State.Place:
                    Place();
                    break;
            }
        }
    }
    #endregion

    private void Copy_hotkey_attempted(InputAction.CallbackContext obj)
    {
        if(control)
            Copy();
    }

    #region Control handlers
    private void Control_released(InputAction.CallbackContext obj)
    {
        control = false;
    }

    private void Control_pressed(InputAction.CallbackContext obj)
    {
        control = true;
    }
    #endregion
    #endregion

    #region functionality
    public void Copy()
    {
        bool cleared = false;

        //variables for center point calculation
        HighAndLow X = new HighAndLow();
        HighAndLow Y = new HighAndLow();
        HighAndLow Z = new HighAndLow();

        foreach (FMInfo selector in SelectorTool.SelectedObjects)
        {
            if (cleared == false)
            {//only clear the inputs after we are sure something else is copied
                copiedObjects.Clear(); //forget about the old copied stuff
                CancelPreview();//incase they are a real programmer who goes ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C, ctrl+V
                cleared = true;
            }

            //save copied objects info
            Transform transform = selector.transform;
            FMPrefab prefabType = selector.GetComponent<FMInfo>().basePrefab;
            XML details = prefabType.ConvertToXML(selector.gameObject);
            CopyInfo copiedObject = new CopyInfo(prefabType, details);

            copiedObjects.Add(copiedObject);

            //center point variable update
            Vector3 position = selector.transform.position;
            X.Set(position.x);
            Y.Set(position.y);
            Z.Set(position.z);

            SelectorTool.selectorTool.Deselect(selector);
        }

        if (cleared)
        {
            Vector3 centerPoint = new Vector3(X, Y.Low, Z); //Note we use the Y.low because the group follows the floor

            foreach (CopyInfo copiedInfo in copiedObjects)
            {
                copiedInfo.CenterPoint = centerPoint;
            }

            centerPointObject.transform.position = centerPoint; //debugging

            currentState = State.PreviewPaste;
            Preview();
        }
    }

    #region Pasting
    #region Preview
    public void MovePreview()
    {
        //calculate center point
        //variables for center point calculation
        HighAndLow X = new HighAndLow();
        HighAndLow Y = new HighAndLow();
        HighAndLow Z = new HighAndLow();

        foreach (FMInfo selection in SelectorTool.SelectedObjects)
        {
            Vector3 position = selection.transform.position;
            X.Set(position.x);
            Y.Set(position.y);
            Z.Set(position.z);
        }

        Vector3 centerPoint = new Vector3(X, Y.Low, Z);

        //
        foreach (FMInfo selection in SelectorTool.SelectedObjects)
        {
            Transform selectedObject = selection.selectedObject.transform;
            selectedObject.parent = groupingArea;
            selectedObject.localPosition = selection.transform.position - centerPoint;
        }

        currentState = State.Place;
    }

    public void Preview()
    {
        foreach(CopyInfo copyInfo in copiedObjects)
        {
            copyInfo.Preview();
        }

        currentState = State.Paste;
    }

    public void CancelPreview()
    {
        if(currentState == State.Place)
        {
            SelectorTool.selectorTool.DeselectAll();
        }

        //destroy objects from grouping object
        for(int i = groupingArea.childCount; i > 0; i--)
        {
            Destroy(groupingArea.GetChild(i - 1).gameObject);
        }

        currentState = State.PreviewPaste;
    }
    #endregion

    #region Post Preview Functionality
    public void Paste()
    {
        if (CollisionDetect.CanPlace)
        {
            foreach (CopyInfo copyInfo in copiedObjects)
            {
                copyInfo.Instanciate();
            }

            CancelPreview();
        }
    }

    public void Place()
    {
        if (CollisionDetect.CanPlace)
        {
            //move real objects to selected objects positions
            foreach(FMInfo selection in SelectorTool.SelectedObjects)
            {
                selection.transform.position = selection.selectedObject.transform.position;
            }


            SelectorTool.selectorTool.DeselectAll();
        }
    }
    #endregion
    #endregion
    #endregion
}