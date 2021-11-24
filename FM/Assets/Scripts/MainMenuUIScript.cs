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
    [Header("Login")]
    public InputField username;
    public InputField password;
    public Text validation;
    public GameObject login;

    [Header("Scroll List")]
    public GameObject gameManager;
    public GameObject panel;
    public string[] nameList;
    string currentItem;

    public GameObject itemTemplate;
    GameObject s;

    string xml = ".XML";

    public Image roomSize;


    // Start is called before the first frame update
    void Start()
    {
        newButton.interactable = false;
        uploadButton.interactable = false;
        loginButton.interactable = false;
        validation.text = "";
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


    }

    public void Login()
    {
        if (username.text == "King" && password.text == "1234")
        {
            newButton.interactable = true;
            uploadButton.interactable = true;
            username.text = "";
            password.text = "";
            validation.text = "Login successful";
            login.SetActive(false);
            
        }
        else
        {
            validation.text = "Wrong username or Password. Try Again.";
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
        //Debug.Log(nameList.Length);
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
        currentItem = nameList[itemIndex];
        Debug.Log(currentItem);
        //FileManager.fileManager.SelectFile(nameList[itemIndex]);
    }

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void DownloadButton()
    {
        //string fileName = FileManager.fileManager.currentSimulation + xml;
        string fileName = currentItem;
        Debug.Log(fileName);
        FileManager.fileManager.DownloadSimulation(fileName);
    }

    public void UploadButton()
    {
        //string fileName = FileManager.fileManager.currentSimulation + xml;
        string fileName = currentItem;
        Debug.Log(fileName);
        FileManager.fileManager.UploadSimulation(fileName);
    }

    public void ChangeImage(Sprite sprite)
    {
        roomSize.sprite = sprite;
    }
}
