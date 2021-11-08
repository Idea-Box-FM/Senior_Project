using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetect : MonoBehaviour
{
    public bool canPlace = true;
    private LevelEditorManager editor;

    void Start()
    {
        //grab the LevelEditorManager component
        editor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelEditorManager>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("GoodPrefab"))
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            editor.collision = this;
            canPlace = false;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("GoodPrefab"))
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            canPlace = true;
        }
    }
}
