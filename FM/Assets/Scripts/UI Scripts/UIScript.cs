using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{


    public void OnButtonHover(TextMeshProUGUI text)
    {
        text.enabled = true;
    }

    public void OffButtonHover(TextMeshProUGUI text)
    {
        text.enabled = false;
    }

}
