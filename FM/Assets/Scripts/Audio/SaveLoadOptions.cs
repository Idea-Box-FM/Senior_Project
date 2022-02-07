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

    //!make sure after pressing the reset preferences button it also adjusts the sliders

    [Header("To Fill")]
    public List<string> prefDefaultKeys = new List<string>();
    public List<string> prefDefaultValues = new List<string>();

    [Header("Display")]
    public List<string> prefKeys = new List<string>();
    public List<string> prefValues = new List<string>();

    public string fileName = "/preferencesCurrent.dat";

    void Start()
    {
        
    }
    
    private void Update()
    {
        //update static options//!issue if more than 1 of these scripts in the scene
        GetOptionsStored.prefKeys = this.prefKeys;
        GetOptionsStored.prefValues = this.prefValues;
    }

    private KeyValuePair<string, string> GetDefaultPref()
    {
        string key = "Default";
        string value = "100";

        return new KeyValuePair<string, string>(key, value);
    }

    public void AddEntry(string newKey, string newValue)//add/update an entry in the lists
    {
        if (prefKeys.Count <= 0)//if there are no entries
        {
            prefKeys.Add(newKey);
            prefValues.Add(newValue);
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
                        //Debug.Log(Time.realtimeSinceStartup + " -> Updated " + newKey + " key with " + newValue + " value");
                        break;
                    }
                }
            }
            else//if there is not an entry
            {
                prefKeys.Add(newKey);
                prefValues.Add(newValue);
            }
        }
    }

    public void ResetAllPrefs()//delete all preferences from all sources
    {
        //set default preferences
        List<string> missingKeys = new List<string>();
        for (int i = 0; i < prefKeys.Count; i++)//loop through keys
        {
            for (int j = 0; j < prefDefaultKeys.Count; j++)//loop through defaults
            {
                if (prefKeys[i] == prefDefaultKeys[j])//if the same key
                {
                    prefValues[i] = prefDefaultValues[j];//update the value to its default
                }
            }
            if (prefDefaultKeys.Contains(prefKeys[i]) == false)//if not containing same key
            {
                missingKeys.Add(prefKeys[i]);//add to warning
                //remove the key and its entry
                prefKeys.RemoveAt(i);
                prefValues.RemoveAt(i);
            }
        }
        if (missingKeys.Count >= 0)//if entries in missing keys
        {
            foreach (string key in missingKeys)//loop through them
            {
                Debug.LogWarning("Unable to get default value of \"" + key + "\", did you set one?");//warning display
            }
        }

        SaveListFile();//save to file

        //Debug.Log("Removed all preferences");
    }

    #region File Management Methods
    public void SaveListFile()//button
    {
        string fileLocation = Application.persistentDataPath + fileName;//file location
        FileStream file;//the file contents

        if (File.Exists(fileLocation))//if the file exists already
            file = File.OpenWrite(fileLocation);//write to it
        else //otherwise
            file = File.Create(fileLocation);//create it

        SortedList<string, string> data = new SortedList<string, string>();//variable to store data written to file
        for (int i = 0; i < prefKeys.Count; i++)//loop through keys
        {
            data.Add(prefKeys[i], prefValues[i]);
        }

        BinaryFormatter bf = new BinaryFormatter();//formatter
        bf.Serialize(file, data);//serialization, will write to file
        file.Close();//close the file

        Debug.Log("Saved file to " + fileLocation);//debug location
    }
    public void LoadListFile()//button
    {
        string fileLocation = Application.persistentDataPath + fileName;//file location
        FileStream file;//the file contents

        if (File.Exists(fileLocation))//if the file exists already
        {
            file = File.OpenRead(fileLocation);//open it and store the data

            BinaryFormatter bf = new BinaryFormatter();//formatter
            SortedList<string, string> data = (SortedList<string, string>)bf.Deserialize(file);//get the data from the file and convert it
            file.Close();//close the file

            for (int i = 0; i < data.Keys.Count; i++)//loop through converted data
            {
                if (prefKeys.Contains(data.Keys[i]) == false)//if there is not the key in the list
                {
                    prefKeys.Add(data.Keys[i]);//store the key
                    prefValues.Add(data.Values[i]);//store the value
                }
                for(int j = 0; j < prefKeys.Count; j++)
                {
                    if(prefKeys[j] == data.Keys[i])//if same key
                    {
                        //convert values to list
                        List<string> newValues = new List<string>();
                        foreach (string value in data.Values)
                            newValues.Add(value);

                        prefValues = newValues;//update it's list
                    }
                }
            }

            //Debug.Log("Loaded file from " + fileLocation);
        }
        else
            Debug.Log("File not found");
    }
    #endregion

    #region Component Management Methods
    public void SetSliderPref(Slider slider)//stores the slider's value //button
    {
        AddEntry(slider.name, slider.value.ToString());

        SaveListFile();
    }

    public void GetSliderPref(Slider slider)//apply the saved preferences to the sliders //button
    {
        LoadListFile();//update values for sure

        //apply the slider's stored preference when the screen is loaded to prevent it from being shown as the wrong value     
        //checks order inversed to have most recent updated last

        if (prefKeys.Contains(slider.name) == true)//if the key exists
        {
            float newValue = float.Parse(prefValues[prefKeys.IndexOf(slider.name)]);//parse needed to convert, otherwise specified cast not valid
            slider.value = newValue;//update the value
        }
    }
    #endregion

    //end
}