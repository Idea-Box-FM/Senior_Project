using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;

/*Flower Box
 * Programmer: Patrick Naatz
 * Purpose create a file capable of storing the data onto a server
 * that will work as a key that will show what files are on the server
 */

public class TableModel : RealmObject
{
    [PrimaryKey]
    public string listName { get; set; }

    public string simulations { get; set; } //all the file names are saved as CSV's because we cant save multiple collections as blobs for some reason

    #region Constructors
    public TableModel() { }

    public TableModel(string listName, string simulations)
    {
        this.listName = listName;
        this.simulations = simulations;
    }
    #endregion
}
