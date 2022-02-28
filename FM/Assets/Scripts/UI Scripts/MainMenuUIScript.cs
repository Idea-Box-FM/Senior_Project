using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;


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
/*
 * Editor Patrick Naatz
 *  Removed a UnityEditor line preventing building from end game function 2/10/2022'
 *  Fixed a bug with currentItem, in future reference please use CurrentItem instead for all uses 2/28/2022
 *  Fixed a bug where reloading the list did not delete the old script 2/28/2022
 *  Fixed a bug where local sims displayed as online sims and vice versa 2/28/2022
 *  Fixed a bug where download button did not update the list 2/28/2022
 *  Added a Delete button function 2/28/2022
 */

public class MainMenuUIScript : MonoBehaviour
{
    [Header("Buttons")]
    public Button downloadButton;
    public Button startButton;
    public Button newButton;
    public Button uploadButton;
    //public Button loginButton;
    //[Header("Login")]
    //public InputField username;
    //public InputField password;
    //public Text validation;
    //public GameObject login;

    [Header("Scroll List")]
    public GameObject gameManager;
    public GameObject LocalPanel;
    //public GameObject OnlinePanel;
    public string[] localSimList;
    public string[] onlineSimList;
    public List<string> simList;
    string currentItem;
    public GameObject itemTemplate;
    GameObject l;
    GameObject o;
    GameObject s;
    int simListLength;


    string xml = ".XML";

    public Image roomSize;

    char[] xmlTrim = { '.', 'X', 'M', 'L' };

    int selectedSim;

    GameObject selectedButton;

    //audio script input
    public PlaySoundEffect audio;

    private bool waited;

    string CurrentItem
    {
        get
        {
            return currentItem ?? FileManager.fileManager.currentSimulation;
        }

        set
        {
            currentItem = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        newButton.interactable = false;
        uploadButton.interactable = false;
        //loginButton.interactable = false;
        //validation.text = "";
        localSimList = FileManager.fileManager.localSimulations;
        onlineSimList = FileManager.fileManager.onlineSimulations;

        selectedSim = 0;


        UpdateList();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(username.text != "" && password.text != "")
        //{
        //    loginButton.interactable = true;
        //}

        



        for (int i = 0; i < LocalPanel.transform.GetChild(0).gameObject.transform.childCount; i++)
        {
            if (i == selectedSim)
            {
                selectedButton = LocalPanel.transform.GetChild(0).gameObject.transform.GetChild(selectedSim).gameObject;
                selectedButton.GetComponent<Button>().image.color = new Color(0, 1, 0, 1);
            }
            else
            {
                selectedButton = LocalPanel.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject;
                selectedButton.GetComponent<Button>().image.color = new Color(1, 1, 1, 0);
            }
        }



    }

    public void Login()
    {
        //if (username.text == "King" && password.text == "1234")
        //{
        //    newButton.interactable = true;
        //    uploadButton.interactable = true;
        //    //username.text = "";
        //    //password.text = "";
        //    validation.text = "Login successful";
        //    login.SetActive(false);
            
        //}
        //else
        //{
        //    validation.text = "Wrong username or Password. Try Again.";
        //}
    }

    public void UpdateList()
    {
        UnloadList();

        localSimList = FileManager.fileManager.localSimulations;

        onlineSimList = FileManager.fileManager.onlineSimulations.ToList().Except(localSimList).ToArray();

        simListLength = localSimList.Length + onlineSimList.Length;

        simList.Clear();
        //combining the two simulation list into one
        for(int i = 0; i < simListLength; i++)
        {
            if(i < localSimList.Length)
            {
                simList.Add(localSimList[i]);
            }
            else if(i >= localSimList.Length)
            {
                simList.Add(onlineSimList[i - localSimList.Length]);
            }
        }

        for (int i = 0; i < simList.Count; i++)
        {
            if (i < localSimList.Length)
            {
                s = Instantiate(itemTemplate, LocalPanel.transform.GetChild(0).transform);
                string simName = simList[i].TrimEnd(xmlTrim);
                s.transform.GetChild(0).GetComponent<TMP_Text>().text = simName;
                s.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.blue;
                s.GetComponent<Button>().AddEventListener(i, ItemClicked);
            }
            else
            {
                s = Instantiate(itemTemplate, LocalPanel.transform.GetChild(0).transform);
                string simName = simList[i].TrimEnd(xmlTrim);
                s.transform.GetChild(0).GetComponent<TMP_Text>().text = simName;
                s.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.black;
                s.GetComponent<Button>().AddEventListener(i, ItemClicked);
            }

            if (simList[i] == FileManager.fileManager.currentSimulation)
                selectedSim = i;
        }

    }

    public void UnloadList()
    {
        foreach(Transform child in LocalPanel.transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }

        //Debug.Log(nameList.Length);
        //for (int i = 0; i < simulationListCount; i++)
        //{
        //    Debug.Log(LocalPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        //   Destroy(LocalPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
        //}
    }

    void ItemClicked (int itemIndex)
    {
        // Debug.Log("Button " + itemIndex + " was clicked");
        currentItem = simList[itemIndex];  // + xml;

        FileManager.fileManager.SelectFile(currentItem);

        selectedSim = itemIndex;

        
        Debug.Log(currentItem);
        //FileManager.fileManager.SelectFile(nameList[itemIndex]);
    }

    #region Helper Methods

    public void ChangeScene(int scene)
    {
        Wait(audio.soundClips[audio.selectedClip].length);
        if(waited == true) SceneManager.LoadScene(scene);
    }

    public void DownloadButton()
    {
        //string fileName = FileManager.fileManager.currentSimulation + xml;
        string fileName = CurrentItem;
        Debug.Log(fileName);
        FileManager.fileManager.DownloadSimulation(fileName);
        UpdateList();
    }

    public void UploadButton()
    {
        //string fileName = FileManager.fileManager.currentSimulation + xml;
        string fileName = CurrentItem;
        Debug.Log(fileName);
        FileManager.fileManager.UploadSimulation(fileName);
    }

    public void DeleteButton()
    {
        string fileName = CurrentItem;
        Debug.Log("deleteing " + fileName);
        FileManager.fileManager.DeleteSimulation(fileName);
        UpdateList();
    }

    public void ChangeImage(Sprite sprite)
    {
        roomSize.sprite = sprite;
    }

    public void playButtonSoundEffect(string caller)
    {
        audio.Play(caller: caller);
    }

    IEnumerator Wait(float delay)
    {
      //   waited = true;
        yield return new WaitForSeconds(delay);
        

    }

    public void EndGame()
    {
        Application.Quit();
    }

    #endregion

   
}
