using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSettingsSO : ScriptableObject
{
    public GameConfig gameConfig { private get; set; }
    public int selectedDifficultyLevel;
    public string playerName;

    public int GetButtonsCount()
    {
        return gameConfig.difficultyLevels[selectedDifficultyLevel].buttonsCount;
    }

    public int GetPointsPerStep()
    {
        return gameConfig.difficultyLevels[selectedDifficultyLevel].pointsPerStep;
    }

    public int GetTimeLimit()
    {
        return gameConfig.difficultyLevels[selectedDifficultyLevel].TimeLimit;
    }

    public bool IsRepeatEnabled()
    {
        return gameConfig.difficultyLevels[selectedDifficultyLevel].repeatModeEnabled;
    }

    public string[] GetLevelNames()
    {
        return gameConfig.difficultyLevels.Select(level => level.name).ToArray();
    }
}
