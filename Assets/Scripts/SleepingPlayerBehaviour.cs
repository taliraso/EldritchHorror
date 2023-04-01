using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingPlayerBehaviour : MonoBehaviour
{
    public bool readyToGetUp;
    public GameObject player;
    public GameObject dog;
    public Transform dogGoTo;
    public float getReadyTime = 2;
    private bool pressed;
    public bool messageUp;

    private void Start()
    {
        StartCoroutine(GetReady());
        if (dog != null)
        {
            dog.GetComponent<DogControls>().setAnimation(4);
        }
    }

    public IEnumerator GetReady()
    {
        //wait for fade in to finish
        yield return new WaitForSeconds(getReadyTime);

        readyToGetUp = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (readyToGetUp && !pressed && !messageUp)
            {
                //get up
                StartCoroutine(GetUp());
                pressed = true;
            }
        }
    }

    public IEnumerator GetUp()
    {
        //animation to get up
        if (dog != null)
        {
            dog.GetComponent<DogControls>().GoToTransfrom(dogGoTo);
        }

        player.SetActive(true);
        Destroy(this.gameObject);
        player.GetComponent<PlayerController>().WakeUpAnimation();
        yield return null;
    }
}
