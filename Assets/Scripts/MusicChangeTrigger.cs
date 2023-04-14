using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [Header("Valence")]

    [SerializeField] private MusicParam valence;

    [Header("Optimism")]

    [SerializeField] private OptimismParam optimism;

    [Header("Lvl_Increase")]

    [SerializeField] private LvlParam lvl_increase;

    //If using a collider to trigger music change
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
        else if (string.Equals(valence, "Hallway_Valence_MX"))
        {
            AudioManager.instance.SetMusicParam(MusicParam.Hallway_Valence_MX);
        }
        else Debug.LogError("Incorrect Music Change Trigger String");
        
    }

    public void ChangeOptimism(string optimism)
    {
        if (string.Equals(optimism, "On"))
        {
            AudioManager.instance.SetOptimismParam(OptimismParam.On);
        }
        else if (string.Equals(optimism, "Off"))
        {
            AudioManager.instance.SetOptimismParam(OptimismParam.Off);
        }
        else Debug.LogError("Incorrect Music Change Trigger String");
    }

    public void ChangeLvlIncrease(string lvl_increase)
    {
        if (string.Equals(lvl_increase, "Off"))
        {
            AudioManager.instance.SetLvlIncreaseParam(LvlParam.Off);
        }
        else if (string.Equals(lvl_increase, "One"))
        {
            AudioManager.instance.SetLvlIncreaseParam(LvlParam.One);
        }
        else if (string.Equals(lvl_increase, "Two"))
        {
            AudioManager.instance.SetLvlIncreaseParam(LvlParam.Two);
        }
        else if (string.Equals(lvl_increase, "Three"))
        {
            AudioManager.instance.SetLvlIncreaseParam(LvlParam.Three);
        }
        else Debug.LogError("Incorrect Music Change Trigger String");
    }

}
