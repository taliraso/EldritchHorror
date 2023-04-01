using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalconyAchievement : MonoBehaviour
{
    public float timer = 0;
    private bool ticking;

    private void OnEnable()
    {
        ticking = true;
    }

    private void OnDisable()
    {
        ticking = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ticking)
        {
            timer += Time.deltaTime;

            if (timer > 60f)
            {
                GameObject.Find("SteamManager").GetComponent<SteamManager>().TriggerAchievement("FRESH_AIR_ACHIEVEMENT");
            }
        }
    }
}
