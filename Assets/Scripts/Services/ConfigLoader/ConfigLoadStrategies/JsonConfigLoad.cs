using UnityEngine;

public class JsonConfigLoad : IConfigLoadStrategy
{
    const string JSON_CONFIG_PATH = "Assets/Configs/config.json";

    public GameConfig LoadConfig(ReadFileFunc readFileFunc)
    {
        string jsonAsString = readFileFunc(JSON_CONFIG_PATH);
        return JsonUtility.FromJson<GameConfig>(jsonAsString);
    }
}
