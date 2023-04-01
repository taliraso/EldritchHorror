using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementTrigger : MonoBehaviour
{
    public enum ACHIEVEMENT
    {
        COFFEE_ACHIEVEMENT,
        PLANT_ACHIEVEMENT,
        FIND_LAMBSHANK_ACHIEVEMENT,
        FETCH_ACHIEVEMENT,
        FRESH_AIR_ACHIEVEMENT,
        RADIO_ACHIEVEMENT,
        ESCAPE_ACHIEVEMENT,
        DEAD_PLANT_ACHIEVEMENT
    }

    public void TriggerAchievement(int achievementIndex)
    {
        string achievementID;

        switch ((ACHIEVEMENT)achievementIndex)
        {
            case ACHIEVEMENT.COFFEE_ACHIEVEMENT:
                achievementID = "COFFEE_ACHIEVEMENT";
                break;

            case ACHIEVEMENT.PLANT_ACHIEVEMENT:
                achievementID = "PLANT_ACHIEVEMENT";
                break;

            case ACHIEVEMENT.FIND_LAMBSHANK_ACHIEVEMENT:
                achievementID = "FIND_LAMBSHANK_ACHIEVEMENT";
                break;

            case ACHIEVEMENT.FETCH_ACHIEVEMENT:
                achievementID = "FETCH_ACHIEVEMENT";
                break;

            case ACHIEVEMENT.FRESH_AIR_ACHIEVEMENT:
                achievementID = "FRESH_AIR_ACHIEVEMENT";
                break;

            case ACHIEVEMENT.RADIO_ACHIEVEMENT:
                achievementID = "RADIO_ACHIEVEMENT";
                break;

            case ACHIEVEMENT.ESCAPE_ACHIEVEMENT:
                achievementID = "ESCAPE_ACHIEVEMENT";
                break;

            case ACHIEVEMENT.DEAD_PLANT_ACHIEVEMENT:
                achievementID = "DEAD_PLANT_ACHIEVEMENT";
                break;

            default:
                print("Invalid Achievement Index");
                return;
        }

        GameObject.Find("SteamManager").GetComponent<SteamManager>().TriggerAchievement(achievementID);
    }
}
