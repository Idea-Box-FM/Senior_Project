using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GetOptionsStored
{
    public static List<string> prefKeys = new List<string>();
    public static List<string> prefValues = new List<string>();
    public static SaveLoadOptions sLOptions;

    public static object GetValuePref(Slider optionElement)//make sure to parse the return
    {
        string value = "Void";

        //apply the slider's stored preference when the screen is loaded to prevent it from being shown as the wrong value

        //checks order inversed to have most recent updated last
        string lastUpdated = "Defaults";

        
        if (prefKeys.Contains(optionElement.name) == true)//if the key exists
        {
            //update the value
            value = prefValues[prefKeys.IndexOf(optionElement.name)];//parse needed to convert, otherwise specified cast not valid
            lastUpdated = "Player Preferences";
        }
        if (sLOptions.prefKeys.Contains(optionElement.name) == true)//if the key exists
        {
            if(sLOptions != null)
                value = sLOptions.prefValues[sLOptions.prefKeys.IndexOf(optionElement.name)];//parse needed to convert, otherwise specified cast not valid
            lastUpdated = "Session Storage";
        }

        //Debug.Log(optionElement.name + " updated via " + lastUpdated);

        return value;
    }
}
