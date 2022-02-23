using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingScrollValue : MonoBehaviour
{
    public Scrollbar scrollbar;
    public int value = 1;

    // Start is called before the first frame update
    void Start()
    {
        scrollbar.value = value;
    }
}
