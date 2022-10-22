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

public enum Status
{
    locked,
    unlocked
}

[CreateAssetMenu(fileName = "NewLevel", menuName = "Scene Data/Level")]
public class Level : GameScene
{
    [Header("Thumbnail")]
    public Sprite levelThumbnail;
    [Header("PlayerScore")]
    public Gamescore[] scores;
    [Header("Status")]
    public Status status;
}
