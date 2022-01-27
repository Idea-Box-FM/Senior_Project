using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveLoadOptions : MonoBehaviour
{
    //place this script on a manager gameobject and use it's methods
    //saves all prefs as strings, then parse them to get their values
    //0th part of key is data type, 1st part is name, 2nd part is instance ID //# <- look for #
    //make sure to set default values within the UI component!

    //!store everything in a file instead of Player Preferences
    //!make sure after pressing the reset preferences button it also adjusts the sliders

    [Header("To Fill")]
    public List<string> prefDefaultKeys = new List<string>();
    public List<string> prefDefaultValues = new List<string>();

    [Header("Display")]
    public int playerPrefAmount = 0;//how many player prefs are there?
    public List<string> prefKeys = new List<string>();
    public List<string> prefValues = new List<string>();

    public List<int> instanceIDs = new List<int>();

    #region Setup Methods
    private void OnEnable()
    {
        LoadListFile();
        GetAllPrefs();//load options
    }
    private void OnDisable()
    {
        GetAllPrefs();
        SetAllPrefs();//save options
    }
    void Start()
    {
        
    }
    #endregion
    
    private void Update()
    {
        
    }

    private KeyValuePair<string, string> GetDefaultPref()
    {
        string key = "Void, Default, 0";
        string value = "100";

        return new KeyValuePair<string, string>(key, value);
    }

    private void AddDefaultPref()//Adds a placeholder entry
    {
        KeyValuePair<string, string> pair = GetDefaultPref();
        int ID = 0;

        //add a first entry
        if (prefKeys.Contains(pair.Key) == false)
            prefKeys.Add(pair.Key);//key
        if (prefValues.Contains(pair.Value) == false)
            prefValues.Add(pair.Value);//value
        if (instanceIDs.Contains(ID) == false)
            instanceIDs.Add(ID);//instance ID
    }

    public void AddEntry(string newKey, string newValue)//add/update an entry in the list
    {
        if (prefKeys.Count <= 0)//if there are no entries
        {
            EntryStore(newKey, newValue);//add one to the list
        }
        else//if there are entries
        {
            if (prefKeys.Contains(newKey))//if an entry already exists under the same ID
            {
                for (int i = 0; i < prefKeys.Count; i++)//loop through list
                {
                    if (prefKeys[i] == newKey)//if same key
                    {
                        prefValues[i] = newValue;//update it
                        //Debug.Log("Updated key with value");
                        Debug.Log(Time.realtimeSinceStartup + " -> Updated " + newKey + " key with " + newValue + " value");
                        break;
                    }
                }
            }
            else//if there is not an entry
            {
                EntryStore(newKey, newValue);//add one to the list
            }
        }
    }

    private void EntryStore(string newKey, string newValue)//add the entry into the proper lists
    {
        //add it to the lists
        prefKeys.Add(newKey);
        prefValues.Add(newValue);

        playerPrefAmount++;

        SaveListFile();
        //Debug.Log("Created key with value");
        Debug.Log(Time.realtimeSinceStartup + " -> Created " + newKey + " key with " + newValue.ToString() + " value");
    }

    #region Button Methods
    public void SetSliderPref(Slider slider)//stores the slider's value
    {
        string newKey = "Float" + ", " + slider.name.Replace(",", "") + ", " + slider.GetInstanceID();//key has sections
        if(instanceIDs.Contains(slider.GetInstanceID()) == false)//if the instance is not already in the list
            instanceIDs.Add(slider.GetInstanceID());//add it to the list for indexing
        string newValue = slider.value.ToString();

        AddEntry(newKey, newValue);
    }

    public void GetSliderPref(Slider slider)//apply the saved preferences to the sliders
    {
        //apply the slider's stored preference when the screen is loaded to prevent it from being shown as the wrong value
        string getKey = "Float" + ", " + slider.name.Replace(",", "") + ", " + slider.GetInstanceID();
        string lastUpdated = "Nothing";
        var prefs = GetAllPrefs();

        //checks order inversed to have most recent updated last
        if (prefDefaultKeys.Contains(getKey) == true)//if default param exists
        {
            slider.value = prefDefaultValues.IndexOf(getKey);
            lastUpdated = "Defaults";
        }
        else
            Debug.LogWarning("Unable to get default value, did you set one?");

        if (prefs.ContainsKey(getKey) == true)//if the key exists
        {
            //update the value
            float newValue = float.Parse(prefs[getKey]);//parse needed to convert, otherwise specified cast not valid
            slider.value = newValue;//update the value
            lastUpdated = "Player Preferences";
        }
        if (prefKeys.Contains(getKey) == true)//if the key exists
        {
            float newValue = float.Parse(prefValues[prefKeys.IndexOf(getKey)]);//parse needed to convert, otherwise specified cast not valid
            slider.value = newValue;//update the value
            lastUpdated = "Session Storage";
        }

        Debug.Log(slider.name + " updated via " + lastUpdated);
    }
    
    public void SetAllPrefs()//store the preferences in the list into the player prefs file
    {
        for (int i = 0; i < prefKeys.Count; i++)
        {
            string[] stringSplit = prefKeys[i].Split(',');//separate parts of key
            switch (stringSplit[0])//data type as a string
            {
                case "Float"://if float data type
                    PlayerPrefs.SetFloat(prefKeys[i], float.Parse(prefValues[i]));//!replace with read in file
                    break;
                case "Int"://if integer data type
                    PlayerPrefs.SetInt(prefKeys[i], int.Parse(prefValues[i]));//!replace with read in file
                    break;
                case "String"://if string data type
                    PlayerPrefs.SetString(prefKeys[i], prefValues[i]);//!replace with read in file
                    break;
                case "Void":
                    //Void, no assignment
                    break;
                default:
                    Debug.LogWarning("Unable to set preference of key \"" + prefKeys[i] + "\"");
                    break;
            }
            //Debug.Log("Saved new " + stringSplit[0] + " with \"" + prefKeys[i] + "\"  key and \"" + prefValues[i].ToString() + "\" value");
        }

        Debug.Log("Saved all preferences");
        PlayerPrefs.Save();//save to disk//!not required?//!replace with read in file
    }

    public void ResetAllPrefs()//delete all preferences from all sources
    {
        //remove player preferences
        PlayerPrefs.DeleteAll();//!replace with read in file
        //clear lists
        prefKeys.Clear();
        prefValues.Clear();
        instanceIDs.Clear();
        playerPrefAmount = 0;

        //remove file
        string fileLocation = Application.persistentDataPath + "/preferences0.dat";
        if(File.Exists(fileLocation) == true){
            File.Delete(fileLocation);
        }

        Debug.Log("Removed all preferences");

        //set default preferences here
        AddDefaultPref();
    }
    #endregion

    public SortedList<string, string> GetAllPrefs()//grab the preferences from the player preferences file
    {
        SortedList<string, string> loadedEntries = new SortedList<string, string>();//store return value
        //loadedEntries.Clear();//clear to refill with updated data

        //!loop through player preferences
        for (int i = 0; i < prefKeys.Count; i++)//! how many entries are in playerprefs?
        {
            string[] stringSplit = prefKeys[i].Split(',');//separate parts of key//#
            switch (stringSplit[0])//data type as a string
            {
                case "Float"://if float data type
                    loadedEntries.Add(/*Key*/prefKeys[i], /*Value*/PlayerPrefs.GetFloat(prefKeys[i], float.Parse(prefValues[i])).ToString());//!replace with read in file
                    break;
                case "Int"://if integer data type
                    loadedEntries.Add(/*Key*/prefKeys[i], /*Value*/PlayerPrefs.GetInt(prefKeys[i], int.Parse(prefValues[i])).ToString());//!replace with read in file
                    break;
                case "String"://if string data type
                    loadedEntries.Add(/*Key*/prefKeys[i], /*Value*/PlayerPrefs.GetString(prefKeys[i], prefValues[i]));//!replace with read in file
                    break;
                case "Void":
                    //Void, no assignment
                    break;
                default:
                    Debug.LogWarning("Unable to get preference of key \"" + prefKeys[i] + "\"");
                    break;
            }
            Debug.Log("Loaded " + stringSplit[0] + " \"" + prefKeys[i] + "\"  key and \"" + prefValues[i] + "\" value");
        }

        for(int i = 0; i<loadedEntries.Count; i++)
        {
            AddEntry(loadedEntries.Keys[i], loadedEntries.Values[i]);
        }

        Debug.Log("Loaded all preferences");

        return loadedEntries;//return in case needed
    }


    #region File Management Methods
    //!Save default preferences from the list to a file
    //!Load default preferences from a file to the list

    private void SaveListFile()
    {
        string fileLocation = Application.persistentDataPath + "/preferencesCurrent.dat";
        FileStream file;

        if (File.Exists(fileLocation)) file = File.OpenWrite(fileLocation);
        else file = File.Create(fileLocation);

        SortedList<string, string> data = GetAllPrefs();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Saved file to " + fileLocation);
    }
    private void LoadListFile()
    {
        string fileLocation = Application.persistentDataPath + "/preferencesCurrent.dat";
        FileStream file;

        if (File.Exists(fileLocation))
        {
            file = File.OpenRead(fileLocation);

            BinaryFormatter bf = new BinaryFormatter();
            SortedList<string, string> data = (SortedList<string, string>)bf.Deserialize(file);
            file.Close();

            for (int i = 0; i < data.Keys.Count; i++)
            {
                if (prefKeys.Contains(data.Keys[i]) == false)
                {
                    prefKeys.Add(data.Keys[i]);//get keys
                    prefValues.Add(data.Values[i]);//get values
                }

                string[] stringSplit = data.Keys[i].Split(',');//separate parts of key
                if (instanceIDs.Contains(int.Parse(stringSplit[2])) == false)
                    instanceIDs.Add(int.Parse(stringSplit[2]));//get instance ids
                    
            }

            Debug.Log("Loaded file from " + fileLocation);
        }
        else
            Debug.Log("File not found");

    }
    #endregion

    //!end
}