using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Flower Box
 * Programmer: Patrick Naatz
 * Intention: Make a simple class to help demonstarate how to convert new MonoBehavior class's to saveable objects
 * This script works along side ScriptableBarell.cs
 */ 

public class Barell : MonoBehaviour
{

    [System.Serializable] public struct SDS
    {
        public int Health;
        public int Flamability;
        public int Reaction;
        public int PersonalProtection;
    }

    public SDS sds;
}