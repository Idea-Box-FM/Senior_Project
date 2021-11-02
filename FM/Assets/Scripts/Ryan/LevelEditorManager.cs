using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelEditorManager : MonoBehaviour
{
    public ItemController[] itemButtons;
    public GameObject[] itemPrefabs;
    public GameObject[] itemExample;
    public int currentButtonPressed;
    public Camera mainCamera;
    public LayerMask deleteMask;
    public LayerMask mask;

    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame && itemButtons[currentButtonPressed].isClicked)
        {
            itemButtons[currentButtonPressed].isClicked = false;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                Instantiate(itemPrefabs[currentButtonPressed], new Vector3(hit.point.x, hit.point.y + itemPrefabs[currentButtonPressed].transform.position.y, hit.point.z),
                    Quaternion.identity);
                Destroy(GameObject.FindGameObjectWithTag("GoodPrefab"));
            }
        }

        if(Mouse.current.middleButton.wasPressedThisFrame)
        {
            Ray deleteRay = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit deleteHit;

            if(Physics.Raycast(deleteRay, out deleteHit, Mathf.Infinity, deleteMask))
            {
                Destroy(deleteHit.transform.gameObject);
            }
        }
    }
}
