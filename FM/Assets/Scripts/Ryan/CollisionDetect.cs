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
            for(int i = 0; i < col.gameObject.transform.childCount; i++)
            {
                col.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.red;
            }
            editor.collision = this;
            canPlace = false;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("GoodPrefab"))
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            for (int i = 0; i < col.gameObject.transform.childCount; i++)
            {
                col.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.red;
            }
            editor.collision = this;
            canPlace = false;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("GoodPrefab"))
        {
            col.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            for (int i = 0; i < col.gameObject.transform.childCount; i++)
            {
                col.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = Color.green;
            }
            canPlace = true;
        }
    }
}
