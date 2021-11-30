using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realms;

public class GameModel : RealmObject
{
    [PrimaryKey]
    public string gamerTag { get; set; }

    public int redScore { get; set; }

    public GameModel() { }

    public GameModel(string gamerTag, int redScore)
    {
        this.gamerTag = gamerTag;
        this.redScore = redScore;
    }
}
