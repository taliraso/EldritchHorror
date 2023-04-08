using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference textboxAudio { get; private set; }
    [field: SerializeField] public EventReference speechBubbleAudio { get; private set; }
    [field: SerializeField] public EventReference radioTextAudio { get; private set; }

    [field: Header("Dog SFX")]
    [field: SerializeField] public EventReference audiodogjump { get; private set; }
    [field: SerializeField] public EventReference audiodogbark { get; private set; }
    [field: SerializeField] public EventReference audiodoghowl { get; private set; }
    [field: SerializeField] public EventReference audiodogslurp { get; private set; }
    [field: SerializeField] public EventReference audiodogwhine { get; private set; }
    [field: SerializeField] public EventReference audiodogmunch { get; private set; }

    [field: Header("Interactables SFX")]
    [field: SerializeField] public EventReference audiodooruse { get; private set; }
    [field: SerializeField] public EventReference audioplayeroutofbed { get; private set; }
    [field: SerializeField] public EventReference audioaura { get; private set; }
    [field: SerializeField] public EventReference audioradioloop { get; private set; }
    [field: SerializeField] public EventReference audioscream1 { get; private set; }
    [field: SerializeField] public EventReference audioscream2 { get; private set; }
    [field: SerializeField] public EventReference audioplayerthrowball { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference neutralTheme { get; private set; }
    [field: SerializeField] public EventReference happyTheme { get; private set; }
    [field: SerializeField] public EventReference sadTheme { get; private set; }


    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;

    }

}
