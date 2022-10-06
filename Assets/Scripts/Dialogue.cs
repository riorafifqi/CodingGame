using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    //public enum CharacterType{  Player, Navi };

    //public CharacterType talkingCharacter;
    [TextArea(3, 10)]
    public string[] sentences;
}
