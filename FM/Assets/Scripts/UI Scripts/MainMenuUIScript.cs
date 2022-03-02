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
 */

public class MainMenuUIScript : MonoBehaviour
{
    [Header("Buttons")]
    public Button downloadButton;
    public Button startButton;
    public Button newButton;
    public Button uploadButton;

    [Header("Scroll List")]
    public GameObject gameManager;
    public GameObject LocalPanel;
    public string[] localSimList;
    public string[] onlineSimList;
    public List<string> simList;
    string currentItem;
    public GameObject itemTemplate;
    GameObject s;
    int simListLength;


    [Header("Miscellaneous")]
    string xml = ".XML";
    public Image roomSize;
    char[] xmlTrim = { '.', 'X', 'M', 'L' };
    int selectedSim;
    GameObject selectedButton;
    //audio script input
    public PlaySoundEffect audio;
    private bool waited;

   

    // Start is called before the first frame update
    void Start()
    {
        newButton.interactable = false;
        uploadButton.interactable = false;
        localSimList = FileManager.fileManager.localSimulations;
        onlineSimList = FileManager.fileManager.onlineSimulations;

        selectedSim = 0;


        UpdateList();
        
    }

    // Update is called once per frame
    void Update()
    {

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

    public void UpdateList()
    {

        localSimList = FileManager.fileManager.localSimulations;

        onlineSimList = FileManager.fileManager.onlineSimulations;


        simListLength = localSimList.Length + onlineSimList.Length;


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

        //this line gets rid of duplicates in the list
        simList = simList.Distinct().ToList<string>();

        //this for loop creates buttons based on the list of simulations
        for (int i = 0; i < simList.Count;i++)
        {
            if(i < localSimList.Length)
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

    /// <summary>
    /// This function gets rid of everything in the list
    /// </summary>
    public void UnloadList()
    {
        //Debug.Log(nameList.Length);
        for (int i = 0; i < localSimList.Length; i++)
        {
            Debug.Log(LocalPanel.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject);
           Destroy(LocalPanel.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject);
        }

        UpdateList();
    }

    /// <summary>
    /// This functio gets called when a user clicks on a Button for the sim list
    /// </summary>
    /// <param name="itemIndex"> the index in the list</param>
    void ItemClicked (int itemIndex)
    {
        // Debug.Log("Button " + itemIndex + " was clicked");
        currentItem = simList[itemIndex]; 

        FileManager.fileManager.SelectFile(currentItem);

        selectedSim = itemIndex;
        
        Debug.Log(currentItem);
    }

    #region Helper Methods

    public void ChangeScene(int scene)
    {
        Wait(audio.soundClips[audio.selectedClip].length);
        if(waited == true) SceneManager.LoadScene(scene);
    }

    public void DownloadButton()
    {
        string fileName = currentItem;
        Debug.Log(fileName);
        FileManager.fileManager.DownloadSimulation(fileName);
    }

    public void UploadButton()
    {
        string fileName = currentItem;
        Debug.Log(fileName);
        FileManager.fileManager.UploadSimulation(fileName);
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
        yield return new WaitForSeconds(delay);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    #endregion

}
