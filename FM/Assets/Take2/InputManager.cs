using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;
    public CameraControl cameraControl;

    bool clickEnabled = true;
    bool selectedEnabled = true;
    public bool SelectEnabled {
        get
        {
            return selectedEnabled;
        }

        set
        {
            selectedEnabled = value;
            DisablePieces();
            ActivatePieces();
        }
    }

    private void Awake()
    {
        if(inputManager == null)
        {
            inputManager = this;
        } else
        {
            Destroy(this);
        }

        cameraControl = new CameraControl();
        SelectEnabled = true;
    }

    private void OnEnable()
    {
        cameraControl.Enable();
    }

    private void Update()
    {
        bool hoveringUI = EventSystem.current.IsPointerOverGameObject();
        if (clickEnabled && hoveringUI)
        {
            clickEnabled = false;
            DisablePieces();
        } else if (!hoveringUI)
        {
            clickEnabled = true;
            ActivatePieces();
        }
    }

    #region Activation/Deactivation
    private void DisablePieces()
    {
        cameraControl.Editor.Spawn.Disable();
        cameraControl.Editor.Select.Disable();
    }

    private void ActivatePieces()
    {
        if (SelectEnabled)
        {
            cameraControl.Editor.Select.Enable();
        }
        else
        {
            cameraControl.Editor.Spawn.Enable();
        }
    }
    #endregion

    private void OnDestroy()
    {
        if (cameraControl != null)
        {
            Debug.Log(gameObject.name + " destroyed input manager");
            cameraControl.Disable();
        }
    }
}
