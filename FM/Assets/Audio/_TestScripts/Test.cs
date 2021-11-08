using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public E inputScript;

    private void Awake()
    {
        inputScript = new E();
    }
    private void OnEnable()
    {
        inputScript.Enable();
    }
    private void OnDisable()
    {
        inputScript.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //buttons (single trigger)
        if (inputScript.Player.Movement.triggered == true)
        {
            Debug.Log("E - Single, true");
        }
        if (inputScript.Player.Movement.triggered == false)
        {
            Debug.Log("E - Single, false");
        }
        //other (constant trigger aka all states in if)
        if (inputScript.Player.Movement.phase == InputActionPhase.Started)
        {
            Debug.Log("E - Constant, true");
        }
        if (inputScript.Player.Movement.phase == InputActionPhase.Waiting)
        {
            Debug.Log("E - Constant, false");
        }


        //Debug.Log("Array Acess: " + inputScript.asset.actionMaps[0].actions[0]);

        //Debug.Log("Direct Acess: " + inputScript.Player.Movement);


    }
}
