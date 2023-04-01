using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTrigger : MonoBehaviour
{
    public bool silentTrigger;

    public void Trigger()
    {
        if (silentTrigger)
        {
            GetComponent<Interactable>().SilentInteract();
        }
        else
        {
            GetComponent<Interactable>().Interact();
        }
    }
}
