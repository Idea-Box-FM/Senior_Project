using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                Instantiate(prefab, new Vector3(hit.point.x, hit.point.y + prefab.transform.position.y, hit.point.z), Quaternion.identity);
            }
        }
    }
}
