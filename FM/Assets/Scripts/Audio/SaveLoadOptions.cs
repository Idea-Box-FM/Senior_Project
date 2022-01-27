using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadOptions : MonoBehaviour
{
    //place this script on a manager gameobject and use it's methods
    //saves all prefs as strings, then parse them to get their values
    //0th part of key is data type, 1st part is name, 2nd part is instance ID
    //make sure to set default values within the UI component!

    public List<string> prefKeys = new List<string>();
    public List<string> prefValues = new List<string>();

    public List<int> instanceIDs = new List<int>();

    private void OnEnable()
    {
        GetAllPrefs();//load options
        DefaultPrefs();
    }
    private void OnDisable()
    {
        SetAllPrefs();//save options
        DefaultPrefs();
    }

    // Start is called before the first frame update
    void Start()
    {
        DefaultPrefs();
    }

    //add/update an entry in the list
    public void AddEntry(string newKey, string newValue)
    {
        if (prefKeys.Count <= 0)//if there are no entries
        {
            EntryStore(newKey, newValue);//add one to the list
        }
        else//if there are entries
        {
            if (prefKeys.Contains(newKey))//if an entry already exists under the same ID
            {
                EntryUpdate(newKey, newValue);//update it
            }
            else//if there is not an entry
            {
                EntryStore(newKey, newValue);//add one to the list
            }
        }
    }
    private void EntryStore(string newKey, string newValue)
    {
        //add it to the lists
        prefKeys.Add(newKey);
        prefValues.Add(newValue);

        //Debug.Log("Created key with value");
        Debug.Log(Time.realtimeSinceStartup + " -> Created " + newKey + " key with " + newValue.ToString() + " value");
    }
    private void EntryUpdate(string newKey, string newValue)
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

    //specific object's data entry methods
    public void SetSliderPref(Slider slider)
    {
        string newKey = "Float" + ", " + slider.name.Replace(",", "") + ", " + slider.GetInstanceID();//key has sections
        if(instanceIDs.Contains(slider.GetInstanceID()) == false)//if the instance is not already in the list
            instanceIDs.Add(slider.GetInstanceID());//add it to the list for indexing
        string newValue = slider.value.ToString();

        AddEntry(newKey, newValue);
    }

    //specific object's data retrieval methods
    public void GetSliderPref(Slider slider)//on load, apply the saved preferences to the sliders
    {
        //apply the slider's stored preference when the screen is loaded to prevent it from being shown as the wrong value
        string getKey = "Float" + ", " + slider.name.Replace(",", "") + ", " + slider.GetInstanceID();
        string lastUpdated = "Nothing";
        var prefs = GetAllPrefs();

        //checks order inversed to have most recent updated last
        if (prefs.ContainsKey(getKey) == true)//if the key exists //!playerPrefs
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

    //preference management methods
    public void SetAllPrefs()//primarily for UI's "save" button
    {
        for (int i = 0; i < prefKeys.Count; i++)
        {
            string[] stringSplit = prefKeys[i].Split(',');//separate parts of key
            switch (stringSplit[0])//data type as a string
            {
                case "Float"://if float data type
                    PlayerPrefs.SetFloat(prefKeys[i], float.Parse(prefValues[i]));
                    break;
                case "Int"://if integer data type
                    PlayerPrefs.SetInt(prefKeys[i], int.Parse(prefValues[i]));
                    break;
                case "String"://if string data type
                    PlayerPrefs.SetString(prefKeys[i], prefValues[i]);
                    break;
            }
            //Debug.Log("Saved new " + stringSplit[0] + " with \"" + prefKeys[i] + "\"  key and \"" + prefValues[i].ToString() + "\" value");
        }
        Debug.Log("Saved all preferences");
        PlayerPrefs.Save();//save to disk
    }

    public void ResetAllPrefs()
    {
        //remove player preferences
        PlayerPrefs.DeleteAll();
        //clear lists
        prefKeys.Clear();
        prefValues.Clear();
        instanceIDs.Clear();
        //remove file
        //!
        Debug.Log("Removed all preferences");

        //set default preferences here
        DefaultPrefs();
    }
    private void DefaultPrefs()
    {
        string key = "Int, Default, 0";
        string value = "100";
        int ID = 0;

        //add a first entry
        if(prefKeys.Contains(key) == false)
            prefKeys.Add(key);//key
        if (prefValues.Contains(value) == false)
            prefValues.Add(value);//value
        if (instanceIDs.Contains(ID) == false)
            instanceIDs.Add(ID);//instance ID
    }

    public SortedList<string, string> GetAllPrefs()
    {
        SortedList<string, string> loadedEntries = new SortedList<string, string>();
        //loadedEntries.Clear();//clear to refill with updated data

        for (int i = 0; i < prefKeys.Count; i++)
        {
            string[] stringSplit = prefKeys[i].Split(',');//separate parts of key
            switch (stringSplit[0])//data type as a string
            {
                case "Float"://if float data type
                    loadedEntries.Add(/*Key*/prefKeys[i], /*Value*/PlayerPrefs.GetFloat(prefKeys[i], float.Parse(prefValues[i])).ToString());
                    break;
                case "Int"://if integer data type
                    loadedEntries.Add(/*Key*/prefKeys[i], /*Value*/PlayerPrefs.GetInt(prefKeys[i], int.Parse(prefValues[i])).ToString());
                    break;
                case "String"://if string data type
                    loadedEntries.Add(prefKeys[i], PlayerPrefs.GetString(prefKeys[i], prefValues[i]));
                    break;
            }
            Debug.Log("Loaded " + stringSplit[0] + " \"" + prefKeys[i] + "\"  key and \"" + prefValues[i] + "\" value");

        }
        //Debug.Log("Loaded all preferences");

        return loadedEntries;//return in case needed
    }

    private void SaveFile()
    {
        string destination = Application.persistentDataPath + "/preferences0.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        SortedList<string, string> data = GetAllPrefs();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();

        Debug.Log("Saved file to " + destination);
    }
    private void LoadFile()
    {
        string destination = Application.persistentDataPath + "/preferences0.dat";
        FileStream file;

        if (File.Exists(destination)) 
            file = File.OpenRead(destination);
        else
        {
            Debug.Log("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SortedList<string, string> data = (SortedList<string, string>)bf.Deserialize(file);
        file.Close();

        if(data != null)
        {
            foreach(string key in data.Keys)
            {
                if(prefKeys.Contains(key) == false)
                    prefKeys.Add(key);//get keys

                string[] stringSplit = key.Split(',');//separate parts of key
                if (instanceIDs.Contains(int.Parse(stringSplit[2])) == false)
                    instanceIDs.Add(int.Parse(stringSplit[2]));//get instance ids
            }
            foreach (string value in data.Values)
            {
                if (prefValues.Contains(value) == false
                    ) prefValues.Add(value);//get values
            }
            
            Debug.Log("Loaded file");
        }
        else
        {
            Debug.Log("Could not parse data from file");
        }
        
    }
}