using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImage : MonoBehaviour
{
    public Image img;
    public List<Sprite> newSprites = new List<Sprite>();
    public int currentSpriteID = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapSprite(int newSpriteID = 0)
    {
        currentSpriteID = newSpriteID;
        img.sprite = newSprites[newSpriteID];
    }
}
