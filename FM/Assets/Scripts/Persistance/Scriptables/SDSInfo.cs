using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDSInfo : MonoBehaviour
{
    public string sdsPath;

    [SerializeField][Range(0,4)] int flamability, reactivity, health;

    public int Flamability
    {
        get
        {
            return flamability;
        }

        set
        {
            flamability = Mathf.Clamp(value, 0, 4);
        }
    }
    public int Reactivity
    {
        get
        {
            return reactivity;
        }

        set
        {
            reactivity = Mathf.Clamp(value, 0, 4);
        }
    }
    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = Mathf.Clamp(value, 0, 4);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
