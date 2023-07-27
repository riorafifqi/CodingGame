using System;
using UnityEngine;

[System.Serializable]
public struct Gamescore
{
    public string name;
    public int totalLine;
    public float time;
    public DateTime date;
}


[System.Serializable]
public struct Objects
{
    public int count;
    public ObjectName obj;
}

public enum Status
{
    locked,
    unlocked
}

public enum ObjectName
{
    Virus,
    Jump,
    Laser,
    Corrupt,
    Pushable,
    Pressure,
    JumpPad
}

[CreateAssetMenu(fileName = "NewLevel", menuName = "Scene Data/Level")]
public class Level : GameScene
{
    [Header("Thumbnail")]
    public Sprite levelThumbnail;
    [Header("Objects")]
    public Objects[] objectList;
    [Header("PlayerScore")]
    public Gamescore[] scores;
    [Header("Status")]
    public Status status;
}
