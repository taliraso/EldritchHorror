using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchBall : MonoBehaviour
{
    public AnimationCurve throwArch;
    public float archHeight;

    public Transform topLeft;
    public Transform bottomRight;

    public float throwDuration = 1f;

    public GameObject dog;
    public float dogWait = 0.5f;

    private Vector3 startingPosition;
    public Transform dogWaitTransform;

    public int numInteract;
    public Interactable[] fetchInteracts;

    public Transform throwStartPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    public void Throw()
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3(transform.position.x + 1f, player.transform.position.y, player.transform.position.z);
        player.GetComponent<PlayerController>().currentFacing = PlayerController.FACING.Left;
        player.GetComponent<PlayerController>().UpdateFacing();

        float randomX = bottomRight.position.x + Random.Range(0f, 1f) * (topLeft.position.x - bottomRight.position.x);
        float randomY = bottomRight.position.y + Random.Range(0f, 1f) * (topLeft.position.y - bottomRight.position.y);

        Vector3 throwPosition = new Vector3(randomX, randomY, 0);

        StartCoroutine(ThrowToPosition(throwPosition, throwDuration));
    }

    public IEnumerator ThrowToPosition(Vector3 newPosition, float duration)
    {
        GameObject player = GameObject.Find("Player");

        player.GetComponent<PlayerController>().isFetching = true;
        player.GetComponent<BoxCollider2D>().enabled = false;

        GetComponent<BoxCollider2D>().enabled = false;

        Vector3 startPosition = throwStartPosition.position;
        float timeElapsed = 0;

        bool dogGone = false;
        GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, newPosition, timeElapsed / duration) + Vector3.up * (throwArch.Evaluate(timeElapsed / duration) * archHeight);
            timeElapsed += Time.deltaTime;

            if (timeElapsed > dogWait && !dogGone)
            {

                GameObject fetchTransformGO = new GameObject("fetchTransformGO");
                fetchTransformGO.transform.position = newPosition;
                DogControls dogControls = dog.GetComponent<DogControls>();
                dogControls.SetupFetch(startingPosition, dogWaitTransform.position, this);
                dogControls.GoToTransfrom(fetchTransformGO.transform);
                Destroy(fetchTransformGO);
                dogGone = true;
            }
            yield return null;
        }
    }

    public void CompletedFetch()
    {
        if (fetchInteracts.Length > numInteract)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().ForceInteraction(fetchInteracts[numInteract]);
        }
        numInteract++;
        GetComponent<SpriteRenderer>().sortingLayerName = "Background";
    }
}
