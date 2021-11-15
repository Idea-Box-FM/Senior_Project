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
 * TODO make the data base connect to the online version of the cluster using config
 */

public class FileManager : MonoBehaviour
{
    #region fields
    public static FileManager fileManager;

    #region path formatting
    const char slash = '\\'; //if this isn't used often enough remove it for memory management

    string path = "";
    [SerializeField] string folderName = "Simulations";
    #endregion

    public string[] localSimulations; //use to be called fileNames

    public string currentSimulation; //use to be current simulation
                                     //consider changing this to a property

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
        path = Directory.GetCurrentDirectory() + slash + folderName;

        LoadLocalFiles();

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
        Debug.Log("All known files: " + table.simulations);
    }

    private void UpdateOnlineSimulations()
    {
        table.simulations.Split(',');
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
    private void LoadLocalFiles()
    {
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] fileInfo = info.GetFiles();

        List<string> localSimulations = new List<string>();
        foreach (FileInfo file in fileInfo)
        {
            localSimulations.Add(file.Name);
        }
        this.localSimulations = localSimulations.ToArray();
    }

    string FormatPath(string fileName)
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
                currentSimulation = PlayerPrefs.GetString("Last Simulation", "fileName");
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// When calling this function pass in the file name without the XML
    /// </summary>
    /// <param name="fileName">if this file name is already in existance then it will automatically select that file</param>
    public void NewFile(string fileName)
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

        xml.ExportXML(fileName + ".XML");

        LoadLocalFiles();
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
            string fileName = FormatPath(simulationName);

            StreamWriter writer = new StreamWriter(fileName);
            {
                writer.Write(fileModel.XMLFile); //because we save the indentation and endlines in the string this automatically formats itself for the XML file
            } writer.Close();

            //update UI
            LoadLocalFiles();
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

        string fileName = FormatPath(simulationName);

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
        realm.Dispose(); //this is required otherwise we destroy our database
    }
}
