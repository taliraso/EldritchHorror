using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [Header("Valence")]

    [SerializeField] private MusicParam valence;

    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.tag.Equals("Player"))
    //    {
    //        AudioManager.instance.SetMusicParam(valence);
    //    }
    //}

    public void ChangeValence(string valence)
    {
        if (string.Equals(valence, "Negative_Valence_MX"))
        {
            AudioManager.instance.SetMusicParam(MusicParam.Negative_Valence_MX);
        }
        else if (string.Equals(valence, "Neutral_Valence_MX"))
        {
            AudioManager.instance.SetMusicParam(MusicParam.Neutral_Valence_MX);
        }
        else if (string.Equals(valence, "Positive_Valence_MX"))
        {
            AudioManager.instance.SetMusicParam(MusicParam.Positive_Valence_MX);
        }
        else Debug.LogError("Incorrect Music Change Trigger String");
        
    }
}
