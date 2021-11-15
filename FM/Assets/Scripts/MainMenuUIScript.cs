using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

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

    public GameObject itemTemplate;
    GameObject s;


    // Start is called before the first frame update
    void Start()
    {
        newButton.interactable = false;
        uploadButton.interactable = false;
        loginButton.interactable = false;
       // Debug.Log(FileManager.fileManager);
        nameList = FileManager.fileManager.localSimulations;
        //currentItem = FileManager.fileManager.currentFile;


        UpdateList();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(username.text != "" && password.text != "")
        {
            loginButton.interactable = true;
        }

        //UpdateList();

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

    public void UpdateList()
    {

        nameList = FileManager.fileManager.localSimulations;

        for (int i = 0; i < nameList.Length; i++)
        {

            s = Instantiate(itemTemplate, panel.transform.GetChild(0).transform);
            s.transform.GetChild(0).GetComponent<Text>().text = nameList[i];

            s.GetComponent<Button>().AddEventListener(i, ItemClicked);

        }
    }

    public void UnloadList()
    {
        Debug.Log(nameList.Length);
        for (int i = 0; i < nameList.Length; i++)
        {
            Debug.Log(panel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
           Destroy(panel.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject);
        }

        UpdateList();
    }

    void ItemClicked (int itemIndex)
    {
        Debug.Log("Button " + itemIndex + " was clicked");
        FileManager.fileManager.SelectFile(nameList[itemIndex]);
    }

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
