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

    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame && itemButtons[currentButtonPressed].isClicked)
        {
            itemButtons[currentButtonPressed].isClicked = false;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(itemPrefabs[currentButtonPressed], new Vector3(hit.point.x, hit.point.y + itemPrefabs[currentButtonPressed].transform.position.y, hit.point.z), Quaternion.identity);
                Destroy(GameObject.FindGameObjectWithTag("GoodPrefab"));
            }
        }
    }
}
