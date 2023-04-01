using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public GameObject firstDoor;
    public GameObject secondDoor;
    public GameObject thirdDoor;

    private void Start()
    {
        firstDoor.SetActive(false);
        secondDoor.SetActive(false);
        thirdDoor.SetActive(false);

        if (!Manager.firstExitMessage)
        {
            firstDoor.SetActive(true);
        }
        else if (!Manager.secondExitMessage)
        {
            secondDoor.SetActive(true);
        }
        else if (!Manager.thirdExitMessage)
        {
            thirdDoor.SetActive(true);
        }
    }

    public void ShowedMessage(int messageNum)
    {
        switch (messageNum)
        {
            case 1:
                Manager.firstExitMessage = true;
                break;

            case 2:
                Manager.secondExitMessage = true;
                break;

            case 3:
                Manager.thirdExitMessage = true;
                break;
        }
    }
}
