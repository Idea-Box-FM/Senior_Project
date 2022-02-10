using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*FlowerBox
 * Edited by: Patrick Naatz
 *  Added some funcitonality needed for the copy/paste functionality 1/31/2022
 *  Organized the new code into sections 1/31/2022
 */
public class CollisionDetect : MonoBehaviour
{
    public bool canPlace = true;
    private LevelEditorManager editor;

    static public bool CanPlace
    {
        get
        {
            foreach(CollisionDetect collisionDetect in FindObjectsOfType<CollisionDetect>())
            {
                if(collisionDetect.canPlace == false)
                {
                    return false;
                }
            }

            return true;
        }
    }

    void Start()
    {
        //grab the LevelEditorManager component
        editor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelEditorManager>();
    }

    #region Triggers
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("GoodPrefab") || col.gameObject.CompareTag("SelectedPrefab"))
        {
            Prevent(col);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("GoodPrefab") || col.gameObject.CompareTag("SelectedPrefab"))
        {
            Permit(col);
        }
    }
    #endregion

    #region Permissions
    void Prevent(Collider col)
    {
        ChangeColor(col.gameObject, Color.red);

        if (col.transform.parent != null)
        {
            if (col.transform.parent.name == "Group")
            {
                foreach (Transform copiedObject in col.transform.parent.GetComponentInChildren<Transform>())
                {
                    ChangeColor(copiedObject.gameObject, Color.red);
                }
            }
        }

        editor.collision = this;
        canPlace = false;
    }

    void Permit(Collider col)
    {
        canPlace = true;

        ChangeColor(col.gameObject, Color.green);

        if (col.transform.parent != null)
        {
            if (col.transform.parent.name == "Group")
            {
                if (CanPlace)
                    foreach (Transform copiedObject in col.transform.parent.GetComponentInChildren<Transform>())
                    {
                        ChangeColor(copiedObject.gameObject, Color.green);
                    }
            }
        }
    }
    #endregion

    static void ChangeColor(GameObject gameObject, Color color)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = color;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
