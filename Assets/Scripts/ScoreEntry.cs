using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text number;
    [SerializeField] private TMP_Text player;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text line;

    public void InsertEntry(string number, string player, string time, string line)
    {
        this.number.text = number;
        this.player.text = player;
        this.time.text = time;
        this.line.text = line;
    }

    public void DeleteEntry()
    {
        Destroy(this.gameObject);
    }
}
