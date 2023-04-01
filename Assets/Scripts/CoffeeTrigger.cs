using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeTrigger : MonoBehaviour
{
    public void TriggerCoffee(int coffeeNum)
    {
        switch (coffeeNum)
        {
            case 1:
                Manager.firstCoffee = true;
                break;

            case 2:
                Manager.secondCoffee = true;
                break;

            case 3:
                Manager.thirdCoffee = true;
                break;

            case 4:
                Manager.fourthCoffee = true;
                break;

            case 5:
                Manager.fifthCoffee = true;
                break;

            case 6:
                if (Manager.sixthCoffee == false)
                {
                    Manager.sixthCoffee = true;

                    if (Manager.firstCoffee && Manager.secondCoffee && Manager.thirdCoffee && Manager.fourthCoffee && Manager.fifthCoffee && Manager.sixthCoffee)
                    {
                        GameObject.Find("SteamManager").GetComponent<SteamManager>().TriggerAchievement("COFFEE_ACHIEVEMENT");
                    }
                }
                break;

            default:
                print("Invalid coffeNum value");
                return;
        }
    }
}
