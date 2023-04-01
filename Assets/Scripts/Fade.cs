using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public enum FADETYPE { In, Out}

    public FADETYPE FadeSetting = FADETYPE.Out;
    public float FadeTime;

    private float timeElapsed = 0;

    public bool endFade = false;

    private void Update()
    {
        if (FadeSetting == FADETYPE.Out)
        {
            GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,Mathf.Lerp(0f, 1f, timeElapsed / FadeTime));
        }
        else if (FadeSetting == FADETYPE.In)
        {
            GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, timeElapsed / FadeTime));
        }

        timeElapsed += Time.deltaTime;
        if (timeElapsed > FadeTime)
        {
            if (!endFade)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
