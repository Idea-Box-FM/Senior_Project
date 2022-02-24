using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Realms;
using System.Linq;

/*Flower Box
 * Made by:Patrick Naatz
 * Intention: make a class capable of persistaning through scenes so that proper file management can be kept
 * 
 * Note: this class uses the singleton pattern
 * 
 * Edited By: Patrick Naatz
 *  Added persistance 11/2/21
 *  Proper Commenting 11/2/21
 *  Added NewFile function 11/8/2021
 *  Added Database connection 11/9/2021 --local database
 *  Changed name of some variables to help with clarification between online and local files
 *  Fixed a bug with realms with singleton pattern 11/27/2021
 *  Updated the new file function to work with room requirements 11/29/2021
 *  Fixed a bug where the simulation was reloaded instead of selected 1/11/2022
 *  Updated to include SDS files inside the SDS folder, please see TODO for more details 2/10/2022
 *  Made sdsPath and format path public for SDSRead script 2/24/2022
 * TODO make the data base connect to the online version of the cluster using config
 * TODO to optimize this script we should refactor it using a Folder class. this is marked as
 * todo and not done because we included a new folder path near the end of the project
 */

public class FileManager : MonoBehaviour
{
    #region fields
    public static FileManager fileManager;

    #region path formatting
    const char slash = '/'; //if this isn't used often enough remove it for memory management

    string path;
    string simulationPath = "";
    public string sdsPath = "";
    [SerializeField] string simulationFolderName = "Simulations";
    [SerializeField] string sdsFolderName = "SDS";
    #endregion

    public string[] localSimulations; //use to be called fileNames

    public string currentSimulation; //use to be current simulation
                                     //consider changing this to a property

    public string[] sdsFiles;

    #region DataBase
    private Realm realm;
    private TableModel table;
    private FileModel fileModel;

    public string[] onlineSimulations;
    #endregion
    #endregion

    #region start
    private void Awake()
    {
        //singleton pattern
        if (fileManager == null)
        {
            DontDestroyOnLoad(this); //this may cause issues if we want to destroy the object but not the script
            fileManager = this;
        }
        else
        {
            Debug.Log("destroying extra file manager on " + gameObject.name);
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        path = Directory.GetCurrentDirectory();
        //Debug.Log("Simulation Save Path: " + path);
        simulationPath = path + slash + simulationFolderName;
        sdsPath = path + slash + sdsFolderName;

        localSimulations = LoadLocalFiles(simulationPath);
        sdsFiles = LoadLocalFiles(sdsPath);

        LoadLastSelectedFile();
    }

    private void OnEnable()
    {
        realm = Realm.GetInstance();

        //realm.Write(() => {
        //    realm.RemoveAll();
        //});

        table = realm.Find<TableModel>("Table");

        if (table == null)
        {
            Debug.Log("Creating table");
            realm.Write(() =>
            {
                table = realm.Add(new TableModel("Table", ""));
            });
        }

        UpdateOnlineSimulations();
        //Debug.Log("All known files: " + table.simulations);
    }

    private void UpdateOnlineSimulations()
    {
        onlineSimulations = table.simulations.Split(',');
    }

    private void LoadLastSelectedFile()
    {
        currentSimulation = PlayerPrefs.GetString("Last Simulation", "");

        if (currentSimulation == "" && localSimulations.Length != 0)
        {
            currentSimulation = localSimulations[0];
        }
    }
    #endregion

    #region Helper functions
    private string[] LoadLocalFiles(string path)
    {
        DirectoryInfo info = new DirectoryInfo(path);

        FileInfo[] fileInfo = info.GetFiles();

        List<string> localFiles = new List<string>();
        foreach (FileInfo file in fileInfo)
        {
            localFiles.Add(file.Name);
        }
        return localFiles.ToArray();
    }

    public string FormatPath(string path, string fileName)
    {
        fileName = path + slash + fileName;

        return fileName;
    }
    #endregion

    #region Button Functions
    /// <summary>
    /// Call this function when you are changing selection of file
    /// </summary>
    /// <param name="fileName">exclude the path. just the filename.XML</param>
    /// <returns> retruns a bool declaring whether the file was found or not</returns>
    public bool SelectFile(string fileName)
    {
        for(int i = 0; i < localSimulations.Length; i++)
        {
            if(localSimulations[i] == fileName)
            {
                PlayerPrefs.SetString("Last Simulation", fileName);
                currentSimulation = fileName;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// When calling this function pass in the file name without the XML
    /// </summary>
    /// <param name="fileName">if this file name is already in existance then it will automatically select that file</param>
    /// <param name="roomSize">x = width, y = height, z = depth</param>
    public void NewFile(string fileName, Vector3 roomSize)
    {
        //stops if file already exists
        foreach(string file in localSimulations)
        {
            if(file == fileName + ".XML")
            {
                SelectFile(file);
                return;
            }
        }

        //creates the new file in the folder
        XML xml = new XML();
        xml.name = fileName;

        XML roomXML = xml.AddChild("Room");
        roomXML.AddAttribute("RoomSize", roomSize.ToString());

        XML Walls = roomXML.AddChild("Walls");
        Walls.AddAttribute("Material", "Wall");

        XML Roof = roomXML.AddChild("Roof");
        Roof.AddAttribute("Material", "Ceiling");

        XML Floor = roomXML.AddChild("Floor");
        Floor.AddAttribute("Material", "Floor");

        xml.ExportXML(fileName + ".XML");

        localSimulations = LoadLocalFiles(simulationPath);
    }


    /// <summary>
    /// Call this function to download a simulation. it wont do anything if the file doesn't exist
    /// </summary>
    /// <param name="simulationName">include .XML</param>
    public void DownloadSimulation(string simulationName)
    {
        fileModel = realm.Find<FileModel>(simulationName);
        if (fileModel == null)
        {
            Debug.Log("Failed to download " + simulationName + "because it does not exist");
        }
        else
        {
            string fileName = FormatPath(simulationPath, simulationName);

            StreamWriter writer = new StreamWriter(fileName);
            {
                writer.Write(fileModel.XMLFile); //because we save the indentation and endlines in the string this automatically formats itself for the XML file
            } writer.Close();

            //update UI
            localSimulations = LoadLocalFiles(simulationPath);
            SelectFile(simulationName);
        }
    }

    /// <summary>
    /// this function will update the file if it already exists in the database
    /// </summary>
    /// <param name="simulationName">include.XML</param>
    public void UploadSimulation(string simulationName)
    {
        //gets or creates file model
        fileModel = realm.Find<FileModel>(simulationName);
        if (fileModel == null)
        {
            realm.Write(() =>
            {
                fileModel = realm.Add(new FileModel(simulationName, ""));
                table.simulations += (table.simulations != "" ? "," : "") + simulationName;
                UpdateOnlineSimulations();
            });
        }

        string fileName = FormatPath(simulationPath, simulationName);

        //converts XML file to single string
        StreamReader myfile = new StreamReader(fileName);
        {
            string XMLFile = myfile.ReadLine();
            string line = "";
            while ((line = myfile.ReadLine()) != null)
            {
                XMLFile += "\n" + line;
            }

            realm.Write(() => { fileModel.XMLFile = XMLFile; });
        }myfile.Close();
    }
    #endregion

    private void OnDisable()
    {
        if(fileManager == this) //do not want to dismiss realm if it is just because of a singleton pattern
            realm.Dispose(); //this is required otherwise we destroy our database
    }
}
