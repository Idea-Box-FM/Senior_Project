using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImage : MonoBehaviour
{
    public Image img;
    public Sprite newSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapSprite(bool storeOld)
    {
        Sprite old = img.sprite;

        img.sprite = newSprite;

        if (storeOld)
            newSprite = old;
    }
}
