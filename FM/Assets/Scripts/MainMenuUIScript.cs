using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    string[] nameList;
    public Text listText;


    // Start is called before the first frame update
    void Start()
    {
        newButton.interactable = false;
        uploadButton.interactable = false;
        loginButton.interactable = false;

        nameList = gameManager.gameObject.GetComponent<FileManager>().fileNames;

        for(int i = 0; i < nameList.Length; i++)
        {
            string tempText = nameList[i];
            Instantiate<Text>(listText);
            listText.text = tempText;
            
        }
        
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
}
