using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Objects")]
    public Camera cam;
    public CameraControl/*InputActionAsset script's class*/ controlScript;//script generated from InputActionAsset
    [Header("Event Variables")]
    public Vector2 horizontalAxis = Vector2.zero;
    public Vector2 verticalAxis = Vector2.zero;
    public Vector2 mouseAxis = Vector2.zero;
    public float placePress = 0;
    public float removePress = 0;
    public float resetPress = 0;
    public float camUnlockPress = 0;

    [Header("Variables")]
    public bool camLocked = true;

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
    }

    // Update is called once per frame
    void Update()
    {
        //**Examples**//!
        //example direct acess, for a specific key
        //if (Keyboard.current.spaceKey/*specific key*/.isPressed == true) { Debug.Log("Pressed \"" + Keyboard.current.spaceKey.name + "\" key"); }

        //example of a specific InputAction as a single trigger
        //if (controlScript.Player/*action map*/.MoveHorizontal/*action*/.triggered == true) { Debug.Log("Pressed \"" + controlScript.Player.MoveHorizontal.activeControl.name + "\" key"); }

        //example of a specific InputAction as a constant trigger (all inputs cause true)
        //if (controlScript.Player/*action map*/.MoveHorizontal/*action*/.phase == InputActionPhase.Started) { Debug.Log("Pressed \"" + controlScript.Player.MoveHorizontal.activeControl.name + "\" key");}

        //example of array acess, using the first InputAction in the first InputActionMap of the InputActionAsset
        //if (controlScript.asset.actionMaps[0].actions[0].triggered == true) { Debug.Log("Pressed \"" + controlScript.asset.actionMaps[0].actions[0].activeControl.name + "\" key"); };


        //**Implementation**//!
        //Move on horizontal axis
        MoveHorizontal();

        //Move on vertical axis
        MoveVertical();

        //Move the camera's rotation based upon the mouse
        Look(false);

        //Place the object
        Place();

        //Remove the object
        Remove();

        //Reset the position
        ResetPosition();

        //Unlock/Lock the camera
        CameraLock();
    }

    #region Event Methods
    private void MoveHorizontal()
    {
        if (controlScript.Player.MoveHorizontal.phase == InputActionPhase.Started)
        {
            //...//!add comments
            horizontalAxis = controlScript.Player.MoveHorizontal.ReadValue<Vector2>();
        }
        else
        {
            horizontalAxis = Vector2.zero;
        }

        var translation = horizontalAxis * Time.deltaTime;//!Factor in where camera is facing
        cam.transform.position += new Vector3(translation.x, 0, translation.y);
    }
    private void MoveVertical()
    {
        if (controlScript.Player.MoveVertical.phase == InputActionPhase.Started)
        {
            //...//!add comments
            verticalAxis = controlScript.Player.MoveVertical.ReadValue<Vector2>();
        }
        else
        {
            verticalAxis = Vector2.zero;
        }

        var translation = verticalAxis * Time.deltaTime;//!Factor in where camera is facing
        cam.transform.position += new Vector3(translation.x, translation.y, 0);
    }
    private void Look(bool invertY)
    {
        if (controlScript.Player.Look.phase == InputActionPhase.Started)
        {
            //Debug.Log(controlScript.Player.Look.ReadValueAsObject());//displays a vector of the cursor's velocity
            mouseAxis = controlScript.Player.Look.ReadValue<Vector2>();

        }
        else
        {

        }

        if (Cursor.visible == false)
        {
            var mouseMovement = mouseAxis * Time.deltaTime * 5;
            if (invertY)
                mouseMovement.y = -mouseMovement.y;
            //var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);//!refactor
            cam.transform.Rotate(-mouseMovement.y, -mouseMovement.x, 0);//!make mouse movement more consistent
        }
        
    }
    private void Place()
    {
        if (controlScript.Player.Place.triggered == true)
        {
            //Debug.Log(controlScript.Player.Place.ReadValueAsObject());//range between 0 and 1, 1 being fully pressed
            //Debug.Log(controlScript.Player.Place.ReadValueAsObject().GetType());//System.single = float
            placePress = controlScript.Player.Place.ReadValue<float>();
            //!place object here
        }
        else
        {

        }
    }
    private void Remove()
    {
        if (controlScript.Player.Remove.triggered == true)
        {
            //Debug.Log(controlScript.Player.Remove.ReadValueAsObject());//range between 0 and 1, 1 being fully pressed
            //Debug.Log(controlScript.Player.Remove.ReadValueAsObject().GetType());//System.single = float
            removePress = controlScript.Player.Remove.ReadValue<float>();
            //!remove object here
        }
        else
        {

        }
    }
    private void ResetPosition()
    {
        if (controlScript.Player.ResetPosition.triggered == true)
        {
            //Debug.Log(controlScript.Player.ResetPosition.ReadValueAsObject());//range between 0 and 1, 1 being fully pressed
            //Debug.Log(controlScript.Player.ResetPosition.ReadValueAsObject().GetType());//System.single = float
            resetPress = controlScript.Player.ResetPosition.ReadValue<float>();

            cam.transform.position = new Vector3(0, 0, 0);
            cam.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {

        }
    }
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
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            Cursor.visible = true;
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