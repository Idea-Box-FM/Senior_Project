using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/*
 * Editor: Patrick Naatz
 *  Changed the order of ButtonClicked function for proper aliasing in LevelEditorManager 11/8/2021
 *  Fixed a bug where you would have 2 examples spawning at the same time 11/8/2021
 */

public class ItemController : MonoBehaviour
{
    //Ryan Consentino

    //the assigned id of the button, needs to be assigned for each button
    public int id;
    //bool to check if button is clicked and if so the prefab is created based on which button is clicked
    public bool isClicked = false;
    //reference to the LevelEditorManager script
    private LevelEditorManager editor;

    // Start is called before the first frame update
    void Start()
    {
        //grab the LevelEditorManager component
        editor = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelEditorManager>();
    }

    //method for when a button is clicked
    public void ButtonClicked()
    {
        //destroys any exmaples that may already exist
        LevelEditorManager.DestroyCurrentExample();

        //set the bool to true so a prefab can spawn
        isClicked = true;

        //set the id of the clicked button
        editor.currentButtonPressed = id;

        //instantiate the example item(green) based on the id of the clicked button
        Instantiate(editor.CurrentPrefab.examplePrefab, new Vector3(0,2,0), Quaternion.identity);
    }
}
