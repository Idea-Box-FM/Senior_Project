using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSimScript : MonoBehaviour
{
    public InputField name;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NewFileButton()
    {
        Debug.Log(name.text);
        FileManager.fileManager.NewFile(name.text);
    }

    public void BackButton()
    {

    }
}