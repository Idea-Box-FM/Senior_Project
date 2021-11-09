using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class ButtonExtension
{
    public static void AddEventListener<T> (this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
       {
           OnClick(param);
       });
    }
}

public class MainMenuUIScript : MonoBehaviour
{
    [Header("Buttons")]
    public Button downloadButton;
    public Button startButton;
    public Button newButton;
    public Button uploadButton;
    public Button loginButton;
    [Header("Input fields")]
    public InputField username;
    public InputField password;

    [Header("Scroll List")]
    public GameObject gameManager;
    public GameObject panel;
    public string[] nameList;
    string currentItem;
    //public Text listText;
    //public Button[] simList;


    // Start is called before the first frame update
    void Start()
    {
        newButton.interactable = false;
        uploadButton.interactable = false;
        loginButton.interactable = false;
       // Debug.Log(FileManager.fileManager);
        nameList = FileManager.fileManager.fileNames;
        //currentItem = FileManager.fileManager.currentFile;

        GameObject itemTemplate = panel.transform.GetChild(0).gameObject;
        GameObject s;

        for(int i = 0; i < nameList.Length; i++)
        {
            s = Instantiate(itemTemplate, panel.transform);
            s.transform.GetChild(0).GetComponent<Text>().text = nameList[i];

            s.GetComponent<Button>().AddEventListener(i, ItemClicked);
            
        }

        Destroy(itemTemplate);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(username.text != "" && password.text != "")
        {
            loginButton.interactable = true;
        }

    }

    public void Login()
    {
        if (username.text == "King" && password.text == "1234")
        {
            newButton.interactable = true;
            uploadButton.interactable = true;
            username.text = "";
            password.text = "";

        }
    }

    void ItemClicked (int itemIndex)
    {
        Debug.Log("Button " + itemIndex + " was clicked");
        FileManager.fileManager.SelectFile(nameList[itemIndex]);
    }
}
