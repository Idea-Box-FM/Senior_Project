using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelector : MonoBehaviour
{
    public Selector[] selectorScripts;


    // Update is called once per frame
    void Update()
    {
        selectorScripts = GameObject.FindObjectsOfType<Selector>();

        for (int i = 0; i < selectorScripts.Length; i++)
        {
            selectorScripts[i].enabled = false;
        }
    }
}
