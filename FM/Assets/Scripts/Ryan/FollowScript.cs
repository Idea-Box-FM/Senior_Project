using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowScript : MonoBehaviour
{
    private LevelEditorManager editor;
    private Camera mainCamera;
    public LayerMask mask;

    void Start()
    {
        editor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelEditorManager>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + editor.itemExample[editor.currentButtonPressed].transform.position.y, hit.point.z);

            //if (Keyboard.current.leftBracketKey.isPressed)
            //{
            //    Vector3 newRotation = new Vector3(0, -90, 0);
            //    transform.eulerAngles = newRotation;
            //}
            //if (Keyboard.current.rightBracketKey.isPressed)
            //{
            //    Vector3 newRotation = new Vector3(0, 90, 0);
            //    transform.eulerAngles = newRotation;
            //}
        }

        
    }
}
