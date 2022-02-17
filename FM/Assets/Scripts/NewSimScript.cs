using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Editied By: Patrick Naatz
 * Changed:
 *  Made the functionality to edit the room size when creating a new simulations 11/30/2021
 *  Added automated selecting 1/11/2022
 *  Dylan 2/17/22: added case check if empty string
 */

public class NewSimScript : MonoBehaviour
{
    #region Fields
    public InputField name;

    public enum RoomSizes
    {
        Small = 0,
        Medium,
        Large
    }

    [Tooltip("Default Room size")]
    [SerializeField] RoomSizes roomSize;

    [Tooltip("Small, Medium, Large")]
    [SerializeField] Vector3[] roomSizes;

    #endregion

    Vector3 SelectedRoomSize
    {
        get
        {
            return roomSizes[(int)roomSize];
        }
    }

    #region SetRoomSize
    /// <summary>
    /// This function only existes because unity forgot about a really useful feature that they said they would add back in 2014
    /// https://forum.unity.com/threads/ability-to-add-enum-argument-to-button-functions.270817/
    /// </summary>
    /// <param name="value">pass in the int value corresponding to the RoomSizes enum</param>
    public void SetRoomSize(int value)
    {
        SetRoomSize((RoomSizes)value);
    }

    public void SetRoomSize(RoomSizes roomSize)
    {
        this.roomSize = roomSize;
    }
    #endregion

    public void NewFileButton()
    {
        if (name.text == "") name.text = "Unnamed Simulation";
        Debug.Log(name.text);
        FileManager.fileManager.NewFile(name.text, SelectedRoomSize);
        FileManager.fileManager.SelectFile(name.text + ".XML");
    }
}
