using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ItemController : MonoBehaviour
{
    public int id;
    public bool isClicked = false;
    private LevelEditorManager editor;

    // Start is called before the first frame update
    void Start()
    {
        editor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelEditorManager>();
    }

    public void ButtonClicked()
    {
        isClicked = true;

        Instantiate(editor.itemExample[id], new Vector3(0,2,0), Quaternion.identity);

        editor.currentButtonPressed = id;
    }
}
