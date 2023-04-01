using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wakeuptimer : MonoBehaviour
{
    private Interactable wakeUpMessage;
    public float waitTime;

    public SleepingPlayerBehaviour player;

    private void Start()
    {
        wakeUpMessage = GetComponent<Interactable>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {

        float timeElapsed = 0;

        while (timeElapsed < waitTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //if player is null, the player is awake and we dont show the message
        if (player != null)
        {
            wakeUpMessage.Interact();
            player.messageUp = true;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (player.messageUp)
            {
                //we need to dismiss the message
                Manager.Instance.ClearText();
                player.messageUp = false;
            }
        }
    }
}
