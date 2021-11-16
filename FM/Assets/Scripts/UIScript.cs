using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{


    public void OnButtonHover(Text text)
    {
        text.enabled = true;
    }

    public void OffButtonHover(Text text)
    {
        text.enabled = false;
    }

}
