using UnityEngine;
using UnityEngine.Rendering;

public class GameScene : ScriptableObject
{
    [Header("Information")]
    public string sceneName;
    public string shortDesc;

    [Header("Sounds")]
    public AudioClip music;

    [Header("Visual")]
    public VolumeProfile volumeProfile;

}