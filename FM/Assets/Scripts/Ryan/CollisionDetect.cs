using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("GoodPrefab"))
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("GoodPrefab"))
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
}
