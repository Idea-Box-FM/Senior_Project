using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraControllerGame : MonoBehaviour
{
    #region Parameters
    [Header("Objects To Assign")]
    [Tooltip("The object that uses the horizontal axis (x) of the view")]
    public GameObject cam;
    [Tooltip("The object that uses the horizontal axis (y) of the view")]
    public GameObject camBody;
    [Tooltip("Script generated from InputActionAsset")]
    public CameraControl/*InputActionAsset script's class*/ controlScript;//script generated from InputActionAsset

    [Header("Event Variables")]
    [Tooltip("Input value for the horizontal plane of the camera")]
    public Vector2 horizontalAxis = Vector2.zero;
    [Tooltip("Input value for the vertical plane of the camera")]
    public Vector2 verticalAxis = Vector2.zero;
    [Tooltip("Value of the mouse's velocity")]
    public Vector2 mouseAxis = Vector2.zero;
    /*[Tooltip("Value of the button press for placement of objects")]
    public float placePress = 0;*/
    /*[Tooltip("Value of the button press for removal of objects")]
    public float removePress = 0;*/
    [Tooltip("Value of the button press for resetting the object")]
    public float resetPress = 0;
    [Tooltip("Value of the button press for unlocking the camera")]
    public float camUnlockPress = 0;

    [Header("Variables")]
    [Tooltip("Speed of the player")]
    public float moveSpeed = 0.01f;
    [Tooltip("Mouse sensitivity")]
    public float mouseSpeed = 3;
    [Tooltip("Store added rotation for each frame")]
    private float yRotation;
    [Tooltip("Is the camera able to be moved?")]
    public bool camLocked = true;
    [Tooltip("The inital position of the camera when the simulation starts, stored before updates")]
    public Vector3 initPos;
    [Tooltip("The inital rotation of the camera when the simulation starts, stored before updates")]
    public Quaternion initRot;

    [Header("Options")]
    [Tooltip("The name of the options slider game object, is a key")]
    public string optionSliderName;
    #endregion

    #region Setup Methods
    private void Awake()
    {
        controlScript = new CameraControl();//make instance of generated script of Input Actions
    }
    private void OnEnable()
    {
        controlScript.Enable();//enable every action map
        //controlScript.Player.Enable();//enable specific action map//Variation 1
        //controlScript.asset.actionMaps[0].Enable();//enabled specific action map//Variation 2
    }
    private void OnDisable()
    {
        controlScript.Disable();//disable every action map
        //controlScript.Player.Disable();//disable specific action map//Variation 1
        //controlScript.asset.actionMaps[0].Disable();//disable specific action map/Variation 2
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //GetAllInputs(true);//!List all inputs

        //store inital values of camera
        initPos = this.transform.position;
        initRot = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //get options
        if (optionSliderName != "")
        {
            if (GetOptionsStored.prefKeys.Count > 0)//if there are items in the list
            {
                int index = GetOptionsStored.prefKeys.IndexOf(optionSliderName);//find index, will return -1 if not found
                if (index < GetOptionsStored.prefValues.Count && index >= 0)//if is within amount of list and is found
                {
                    mouseSpeed = float.Parse((string)GetOptionsStored.prefValues[index]) / 100;//get volume from options and set the value
                }
                else
                    Debug.Log("Unable to find " + optionSliderName + " in current list of keys");
            }
        }
        else
            Debug.LogError("Assign optionSlider on \"" + gameObject.name + "\" game object");


        #region Examples
        //example direct acess, for a specific key
        //if (Keyboard.current.spaceKey/*specific key*/.isPressed == true) { Debug.Log("Pressed \"" + Keyboard.current.spaceKey.name + "\" key"); }

        //example of a specific InputAction as a single trigger
        //if (controlScript.Player/*action map*/.MoveHorizontal/*action*/.triggered == true) { Debug.Log("Pressed \"" + controlScript.Player.MoveHorizontal.activeControl.name + "\" key"); }

        //example of a specific InputAction as a constant trigger (all inputs cause true)
        //if (controlScript.Player/*action map*/.MoveHorizontal/*action*/.phase == InputActionPhase.Started) { Debug.Log("Pressed \"" + controlScript.Player.MoveHorizontal.activeControl.name + "\" key");}

        //example of array acess, using the first InputAction in the first InputActionMap of the InputActionAsset
        //if (controlScript.asset.actionMaps[0].actions[0].triggered == true) { Debug.Log("Pressed \"" + controlScript.asset.actionMaps[0].actions[0].activeControl.name + "\" key"); };
        #endregion

        #region Implementation
        MoveHorizontal();

        Look();

        CameraLock();
        #endregion
    }

    #region Event Methods
    /// <summary>
    /// Move on the horizontal axis (left or right, forward or back)
    /// </summary>
    private void MoveHorizontal()
    {
        horizontalAxis = controlScript.Player.MoveHorizontal.ReadValue<Vector2>();//get input
        Vector3 horiAxisFix = new Vector3(horizontalAxis.x, 0, horizontalAxis.y);//convert to horizontal plane for movement
        Vector3 moveVelocity = (camBody.transform.rotation * horiAxisFix.normalized) * moveSpeed;//get velocity to apply

        camBody.transform.position += moveVelocity;//apply to object
    }
    
    /// <summary>
    /// Move the camera's rotation based upon the mouse
    /// </summary>
    private void Look()
    {
        if (camLocked == false)
        {
            //use 2 objects to have each axis represented
            mouseAxis = controlScript.Player.Look.ReadValue<Vector2>();//get input
            Vector2 mouseAxisFix = new Vector2(mouseAxis.y, mouseAxis.x);//correct values of look are swapped
            Vector2 mouseVelocity = mouseAxisFix.normalized * mouseSpeed;//get rotation to apply

            //limit rotation
            yRotation -= mouseVelocity.x;
            yRotation = Mathf.Clamp(yRotation, -90, 90);//clamp to ceiling and floor

            //apply rotation
            cam.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
            camBody.transform.Rotate(new Vector3(0, mouseVelocity.y, 0));
        }
    }
    
    /// <summary>
    /// Unlock or lock the camera's rotation
    /// </summary>
    private void CameraLock()
    {
        if (controlScript.Player.CameraLock.phase == InputActionPhase.Started)
        {
            //Debug.Log(controlScript.Player.CameraLock.ReadValueAsObject());//range between 0 and 1, 1 being fully pressed
            //Debug.Log(controlScript.Player.CameraLock.ReadValueAsObject().GetType());//System.single = float
            camUnlockPress = controlScript.Player.CameraLock.ReadValue<float>();

            if (camUnlockPress > .5)//if more than 1/2 pressed
            {
                Cursor.visible = false;
                camLocked = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            Cursor.visible = true;
            camLocked = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    #endregion

    #region Helper Methods
    
    #endregion

    #region Display methods
    public List<string> GetAllInputs(bool logToConsole)
    {
        List<string> inputs = new List<string>();
        foreach (InputActionMap IAM in controlScript.asset.actionMaps)
        {
            inputs.Add("ActionMap: " + IAM.name);
            foreach (InputAction IA in IAM)
            {
                inputs.Add("Action: " + IA.name);
                foreach (InputBinding B in IA.bindings)
                {
                    string controller = "";
                    string key = "";
                    string controlType = "";
                    if (B.path.Split('/').Length > 1)//if has controller and its binding
                    {
                        controller = B.path.Split('/')[0];
                        key = B.path.Split('/')[1];
                        inputs.Add("Binding: " + key);
                    }
                    else//is Control Type otherwise
                    {
                        controlType = B.path;
                    }
                }
            }
        }

        if (logToConsole == true)
        {
            string display = "";
            foreach (string input in inputs)
                display += input + ", ";
            Debug.Log(display);
        }

        return inputs;
    }
    public void RespondToInput(string input)
    {
        Debug.Log(input);
    }
    #endregion
}