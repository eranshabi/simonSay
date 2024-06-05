using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LeaderboardService
{
    private string filePath;
    private static LeaderboardModel leaderboardData;
    private static bool externalDataLoaded = false;
    private static ScoreData lastAddedScoreDataInCurrentSeassion;
    [SerializeField] private LeaderboardItem LeaderboardItemPrefab;
    [SerializeField] private GameObject LeaderboardItemsContainer;

    public LeaderboardService(string dataPath)
    {
        filePath = dataPath + "/leaderboard.json";
    }

    public void LoadAllData()
    {
        if (!externalDataLoaded)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                leaderboardData = JsonUtility.FromJson<LeaderboardModel>(json);
            }
            else
            {
                leaderboardData = new LeaderboardModel { scoresData = new List<ScoreData>() };
            }
            externalDataLoaded = true;
        }
    }

    public List<ScoreData> GetData()
    {
        return leaderboardData.scoresData;
    }

    public void AddScoreData(ScoreData scoreData)
    {
        LoadAllData();
        lastAddedScoreDataInCurrentSeassion = scoreData;
        leaderboardData.scoresData.Add(scoreData);
        SaveLeaderboardData();
    }

    private void SaveLeaderboardData()
    {
        string json = JsonUtility.ToJson(leaderboardData);
        File.WriteAllText(filePath, json);
    }

    public ScoreData GetNewestScoreData()
    {
        return lastAddedScoreDataInCurrentSeassion;
    }
}

[Serializable]
class LeaderboardModel
{
    public List<ScoreData> scoresData;
}

[Serializable]
public class ScoreData
{
    public string name;
    public int score;
}