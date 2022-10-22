using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct Gamescore
{
    public string name;
    public int totalLine;
    public float time;
    public DateTime date;
}

[CreateAssetMenu(fileName = "NewLevel", menuName = "Scene Data/Level")]
public class Level : GameScene
{
    [Header("Thumbnail")]
    public Sprite levelThumbnail;
    [Header("PlayerScore")]
    public Gamescore[] scores;
}
