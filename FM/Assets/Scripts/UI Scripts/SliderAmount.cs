using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderAmount : MonoBehaviour
{
    public Slider audioSlider;
    string sliderAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sliderAmount = audioSlider.value.ToString();
        //gameObject.GetComponent<Text>().text = sliderAmount;
        gameObject.GetComponent<TMP_Text>().text = sliderAmount;
    }
}
