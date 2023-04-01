using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    public uint appId;
    public Steamworks.SteamServerInit init;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Steamworks.SteamUserStats.OnAchievementProgress += AchievementChanged;

        try
        {
            Steamworks.SteamClient.Init(appId, true);

            print("Steam Client Initiated");
            print("Hi there, " + Steamworks.SteamClient.Name);
            print("SteamId: " + Steamworks.SteamClient.SteamId);

            //foreach (Steamworks.Data.Achievement achievement in Steamworks.SteamUserStats.Achievements)
            //{
            //    print(achievement.Identifier);
            //    print(achievement.State);
            //}

            Steamworks.SteamServer.Init(appId, init, true);
            //TriggerAchievement("TestAchievement");

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("Couldn't initialize Steam Client");
        }
    }

    private void OnDisable()
    {
        //ClearAllAchievements();

        if (Steamworks.SteamClient.IsValid)
        {
            Steamworks.SteamClient.Shutdown();
        }
        if (Steamworks.SteamServer.IsValid)
        {
            Steamworks.SteamServer.Shutdown();
        }
        print("Shutdown Steam Client and Server");
    }

    private void Update()
    {
        //Steamworks.SteamClient.RunCallbacks();
    }

    public void ClearAllAchievements()
    {
        print("Clearing all achievements");
        foreach (Steamworks.Data.Achievement achievement in Steamworks.SteamUserStats.Achievements)
        {
            achievement.Clear();
        }
    }

    public void TriggerAchievement(string achievementID)
    {
        if (Steamworks.SteamClient.IsValid && Steamworks.SteamServer.IsValid)
        {
            foreach (Steamworks.Data.Achievement achievement in Steamworks.SteamUserStats.Achievements)
            {
                if (achievement.Identifier == achievementID && achievement.State == false)
                {
                    achievement.Trigger();
                }
            }

            foreach (Steamworks.Data.Achievement achievement in Steamworks.SteamUserStats.Achievements)
            {
                print(achievement.Identifier);
                print(achievement.Name);
                print(achievement.State);
            }
        }
    }

    private void AchievementChanged(Steamworks.Data.Achievement ach, int currentProgress, int progress)
    {
        if (ach.State)
        {
            Debug.Log($"{ach.Name} WAS UNLOCKED!");
        }
    }
}
