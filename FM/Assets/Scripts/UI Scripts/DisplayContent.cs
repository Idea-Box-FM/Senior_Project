using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayContent : MonoBehaviour
{
    public GameObject obj;
    public Image billBoardImage;
    public Sprite[] imageList;


    // Update is called once per frame
    void Update()
    {
        if(obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.HealthHazard)
        {
            billBoardImage.sprite = imageList[0];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.Flammability)
        {
            billBoardImage.sprite = imageList[1];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.CompressedGas)
        {
            billBoardImage.sprite = imageList[2];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.Corrosive)
        {
            billBoardImage.sprite = imageList[3];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.Explosive)
        {
            billBoardImage.sprite = imageList[4];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.Oxidizers)
        {
            billBoardImage.sprite = imageList[5];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.Environmental)
        {
            billBoardImage.sprite = imageList[6];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.AcuteToxicity)
        {
            billBoardImage.sprite = imageList[7];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.Other)
        {
            billBoardImage.sprite = imageList[8];
        }
        if (obj.GetComponent<ObjectContents>().currentContent == ObjectContents.Contents.None)
        {
            billBoardImage.sprite = imageList[9];
        }
    }
}
