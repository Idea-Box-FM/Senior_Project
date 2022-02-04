using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*FLOWERBOX
 * Programmer: Patrick Naatz
 * Purpose: make a script capable on handling copying and pasting of multiple objects
 * TODO remove debugging pieces cignified by //Debugging
 * Please test persistance, load scene use copy, exit simulation and relaunch, ensuring copy and paste still works
 * 
 * Edited: Pat Naatz
 *  Changed FMPrefabList to singleton pattern 2/2/2022
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
        Paste
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
        if (currentState == State.Paste)
            Paste();
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

        foreach(Selector selector in FindObjectsOfType<Selector>())
        {
            if (selector.isSelected)
            {
                if(cleared == false)
                {//only clear the inputs after we are sure something else is copied
                    copiedObjects.Clear(); //forget about the old copied stuff
                    CancelPreview();//incase they are a real programmer who goes ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C,ctrl+C, ctrl+V
                    cleared = true;
                }

                //save copied objects info
                Transform transform = selector.transform;
                GameObject parent = transform.parent.gameObject;
                FMPrefab prefabType = prefabList.GetPrefabType(parent);
                XML details = prefabType.ConvertToXML(parent);
                CopyInfo copiedObject = new CopyInfo(prefabType, details);

                copiedObjects.Add(copiedObject);

                //center point variable update
                Vector3 position = transform.position;
                X.Set(position.x);
                Y.Set(position.y - prefabType.height / 2);
                Z.Set(position.z);

                selector.Deselect();
            }
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
        //destroy objects from grouping object
        for(int i = groupingArea.childCount; i > 0; i--)
        {
            Destroy(groupingArea.GetChild(i - 1).gameObject);
        }

        currentState = State.PreviewPaste;
    }
    #endregion

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
    #endregion
    #endregion
}