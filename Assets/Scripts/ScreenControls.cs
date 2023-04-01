using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControls : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Screen.fullScreen)
            {
                Screen.SetResolution(960, 540, false);
            }
            else
            {
                var newx = Display.main.systemWidth;
                var newy = Display.main.systemHeight;
                Screen.SetResolution(newx, newy, true);
            }
        }
    }
}
