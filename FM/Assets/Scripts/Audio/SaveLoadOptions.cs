using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadOptions : MonoBehaviour
{
    //place this script on the back button and call it
    //place this script on an option enter button and call it

    //try to save all prefs as strings, then parse them to get their values
    //find a way of also storing preferences within a string

    public enum PrefType
    {
        Float,
        Int,
        String
    }

    public List <string> prefKeys = new List<string>();
    public List<string> prefValues = new List<string>();
    int iterator = 0;

    private void OnEnable()
    {
        LoadPrefs();//load options
    }
    private void OnDisable()
    {
        SavePrefs();//save options
    }

    // Start is called before the first frame update
    void Start()
    {
        prefKeys.Clear();//clear on play to store new values //!temp?
        prefValues.Clear();//clear on play to store new values //!temp?
    }

    public void AddEntry(string key, string value)//1st part of key is data type, 2nd part is Name and ID if needed, 3rd part is data
    {
        //look if an entry already exists under the same ID, if so replace it

        //if there is not an entry, add one to the list

        prefKeys.Add(key);
        prefValues.Add(value);
    }
    public void AddEntry(List<string> keys, List<string> values)//1st part of key is data type, 2nd part is Name and ID if needed, 3rd part is data
    {
        //look if an entry already exists under the same ID, if so replace it

        //if there is not an entry, add one to the list

        prefKeys.AddRange(keys);
        prefValues.AddRange(values);
    }

    public void StoreSliderPref(Slider value)
    {
        //1st part of key is data type, 2nd part is Name and ID if needed, 3rd part is data
        AddEntry(key: "Float" + ", " + value.name + ", " + iterator, value: value.value.ToString());//!placeholder

        Debug.Log("Stored a value of " + value.value + " from the \"" + value.name +"\" game object");

        //SavePrefs();
    }
    //methods in comment below should be similar to the one above
    /*public void SaveTextPrefs(List<Text> valuesToSave)
    {

    }
    public void SaveTextPrefs(List<TextMeshPro> valuesToSave)
    {

    }

    public void SaveInputPrefs(List<InputField> valuesToSave)
    {

    }*/


    public void ApplySliderPref()//on load, apply the saved preferences to the sliders
    {
        //apply the slider's stored preference when the screen is loaded to prevent it from being shown as the wrong value
    }
    //make methods that correspond to elements similar to the one above

    public void SavePrefs()//primarily for UI's "save" button
    {
        //PlayerPrefs.Save();//save to disk//!do only at end when everything works
    }

    public void LoadPrefs()
    {
        //loop through preference lists (should be same size)
        //extract which data type it is with the below switch case
        /*switch (stringSplit[0])//data type as a string
        {
            case "Float"://if float data type
                //PlayerPrefs.SetFloat(prefKeys[i], float.Parse(prefValues[i]));
                break;
            case "Int"://if integer data type
                //PlayerPrefs.SetInt(prefKeys[i], int.Parse(prefValues[i]));
                break;
            case "String"://if string data type
                //PlayerPrefs.SetString(prefKeys[i], prefValues[i]);
                break;
        }*/
        //apply the loaded preferences to the required objects
    }

    //First idea below (datatype based)
    /*
    public List<float> savedFloats = new List<float>();
    public List<int> savedInts = new List<int>();
    public List<string> savedStrings = new List<string>();
    
    public void SaveFloatPrefs(List<float> floatsToSave)
    {
        //save options
        Debug.Log("Saving " + null + " to disk");
        SavePrefs();
    }
    public void SaveIntPrefs(List<int> intsToSave)
    {
        //save options
        Debug.Log("Saving " + null + " to disk");
        SavePrefs();
    }
    public void SaveStringPrefs(List<string> stringsToSave)
    {
        //save options
        Debug.Log("Saving " + null + " to disk");
        SavePrefs();
    }
    public void LoadPrefs()
    {
        //first method
        //PlayerPrefs.GetFloat();//load Float from disk
        //PlayerPrefs.GetInt();//load Int from disk
        //PlayerPrefs.GetString();//load String from disk
    }
    */
}
