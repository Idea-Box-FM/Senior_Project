using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlaySelector : MonoBehaviour
{
    public PlaySelector[] selectorScripts;


    // Update is called once per frame
    void Update()
    {
        selectorScripts = GameObject.FindObjectsOfType<PlaySelector>();

        for (int i = 0; i < selectorScripts.Length; i++)
        {
            selectorScripts[i].enabled = false;
        }
    }
}
