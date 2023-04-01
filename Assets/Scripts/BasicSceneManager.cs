using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSceneManager : MonoBehaviour
{
    public GameObject controlsSplash;
    public GameObject otherStuff;
    public float waitTime = 5f;

    public void GoToScene(string sceneName)
    {
        StartCoroutine(DelayLoad(sceneName, waitTime));
    }

    public IEnumerator DelayLoad(string sceneName, float time)
    {
        yield return new WaitForSeconds(1f);

        controlsSplash.SetActive(true);
        otherStuff.SetActive(false);

        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneName);
    }
}
