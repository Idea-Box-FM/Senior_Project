using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelector : MonoBehaviour
{
    public Selector[] selectorScripts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
