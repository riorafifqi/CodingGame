using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Level1TutorialManager : MonoBehaviour
{
    public List<GameObject> listGO;
    public Level level;

    public GameObject tutorial;
    public Animator player;

    public TimelineAsset playerTrack;
    public PlayableDirector playableDirector;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Skin").GetComponent<Animator>();
        Debug.Log(player);

        TrackAsset track = null;
        foreach (var t in playerTrack.GetOutputTracks())
        {
            if (t.name == "Player Track")
            {
                track = t;
                break;
            }
        }

        playableDirector.SetGenericBinding(track, player);
    }

    public void DisableGO()
    {
        foreach(var i in listGO)
        {
            i.SetActive(false);
        }
    }

    public void EnableGO()
    {
        foreach(var i in listGO)
        {
            i.SetActive(true);
        }
    }
}
