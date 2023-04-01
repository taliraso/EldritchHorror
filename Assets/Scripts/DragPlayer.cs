using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPlayer : MonoBehaviour
{
    public PlayerController player;
    public DogControls dog;
    public float delayStart;
    public Interactable interactable;

    public void Drag()
    {
        StartCoroutine(DragDelayed());
    }

    private IEnumerator DragDelayed()
    {
        yield return new WaitForSeconds(delayStart);

        player.GetComponentInChildren<SpriteRenderer>().enabled = false;
        interactable.Interact();
    }
}
