using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/*Flower Box
 * Made by:Patrick Naatz
 * Intention: make a class capable of persistaning through scenes so that proper file management can be kept
 * 
 * Note: this class uses the singleton pattern
 * 
 * Edited By: Patrick Naatz
 *  Added persistance 11/2/21
 *  Proper Commenting 11/2/21
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

    public string[] fileNames; //consider renaming to simulations or simulationFiles

    public string currentFile; //consider changing this to a property
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

    private void LoadLastSelectedFile()
    {
        currentFile = PlayerPrefs.GetString("Last Simulation", "");

        if (currentFile == "" && fileNames.Length != 0)
        {
            currentFile = fileNames[0];
        }
    }

    private void LoadLocalFiles()
    {
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] fileInfo = info.GetFiles();

        List<string> fileNames = new List<string>();
        foreach (FileInfo file in fileInfo)
        {
            fileNames.Add(file.Name);
        }
        this.fileNames = fileNames.ToArray();
    }

    #endregion

    /// <summary>
    /// Call this function when you are changing selection of file
    /// </summary>
    /// <param name="fileName">exclude the path. just the filename.XML</param>
    /// <returns> retruns a bool declaring whether the file was found or not</returns>
    public bool SelectFile(string fileName)
    {
        for(int i = 0; i < fileNames.Length; i++)
        {
            if(fileNames[i] == fileName)
            {
                currentFile = PlayerPrefs.GetString("Last Simulation", "fileName");
                return true;
            }
        }

        return false;
    }


    //these comments are here incase we do decide to use online
    //public class FileData
    //{
    //    string xml;
    //    string fileName;
    //}

    //IEnumerator Download(string id, System.Action<FileData> callback = null)
    //{
    //    using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/plummies/" + id))
    //    {
    //        yield return request.SendWebRequest();
    //        if (request.isNetworkError || request.isHttpError)
    //        {
    //            Debug.Log(request.error);
    //            if (callback != null)
    //            {
    //                callback.Invoke(null);
    //            }
    //        }
    //        else
    //        {
    //            if (callback != null)
    //            {
    //                callback.Invoke(FileData.Parse(request.downloadHandler.text));
    //            }
    //        }
    //    }
    //}
}
