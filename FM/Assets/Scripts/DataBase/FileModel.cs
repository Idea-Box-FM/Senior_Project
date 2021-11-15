using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;

/*Flower Box
 * Programmer: Patrick Naatz
 * Purpose create a file capable of storing the data onto a server
 */

public class FileModel : RealmObject
{
    [PrimaryKey]
    public string fileName { get; set; }

    public string XMLFile { get; set; }//the whole file saved as a single line including tabation and line endings

    #region Constructors
    public FileModel() { }

    public FileModel(string fileName, string XMLFile)
    {
        this.fileName = fileName;
        this.XMLFile = XMLFile;
    }
    #endregion
}
