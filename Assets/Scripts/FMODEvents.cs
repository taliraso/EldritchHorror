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

    [field: Header("Music")]

    [field: SerializeField] public EventReference music { get; private set; }


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
