using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{
    [SerializeField] string path;
    string[] fileNames;

    string currentFile;

    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] fileInfo = info.GetFiles();

        List<string> fileNames = new List<string>();
        foreach (FileInfo file in fileInfo)
        {
            fileNames.Add(file.Name);
        }
        this.fileNames = fileNames.ToArray();

        currentFile = PlayerPrefs.GetString("Last Simulation", "");
    }

    void AddFile()
    {

    }

    public class FileData
    {
        string xml;
        string fileName;
    }

    IEnumerator Download(string id, System.Action<FileData> callback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/plummies/" + id))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if (callback != null)
                {
                    callback.Invoke(null);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke(FileData.Parse(request.downloadHandler.text));
                }
            }
        }
    }
}
