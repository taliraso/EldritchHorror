using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RooftopCameraControl : MonoBehaviour
{
    public GameObject player;
    private bool isEnd;
    public GameObject credits;
    public float creditsMin = -10f;
    public float creditsMax = 10f;
    public float creditsTime = 20f;

    void Update()
    {
        if (!isEnd)
        {
            float newy = player.transform.position.y + 3;

            newy = Mathf.Clamp(newy, 0f, 3f);

            transform.position = new Vector3(transform.position.x, newy, transform.position.z);
        }
    }

    public void End()
    {
        isEnd = true;
        StartCoroutine(PanUp());
    }

    private IEnumerator PanUp()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().isTransitioning = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(30f, 15f, -10f);
        float timeElapsed = 0f;
        float duration = 10f;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (timeElapsed / duration));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        //Do a rolling credits
        credits.SetActive(true);

        Transform creditsChild = credits.transform.GetChild(0);

        startPosition = new Vector3(0f, creditsMin, 0f);
        endPosition = new Vector3(0f, creditsMax, 0f);
        timeElapsed = 0f;

        while (timeElapsed < creditsTime)
        {
            creditsChild.transform.localPosition = Vector3.Lerp(startPosition, endPosition, (timeElapsed / creditsTime));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        SceneManager.LoadScene("_Menu");
    }
}
