using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("gameConfig")]
public class GameConfig
{
    [XmlArray("difficultyLevels")] // This could have been implemented as a dictionary, but decided against it as name is in plain english and it will make localization harder later
    [XmlArrayItem("difficultyLevel")]
    public DifficultyLevel[] difficultyLevels;
}

[Serializable]
public class DifficultyLevel
{
    [XmlAttribute] public string name;
    [XmlAttribute] public int buttonsCount;
    [XmlAttribute] public int pointsPerStep;
    [XmlAttribute] public int TimeLimit;
    [XmlAttribute] public bool repeatModeEnabled;
    [XmlAttribute] public float gameSpeed;

    // This implementation of Equals helps us in tests to compare expected and actual config
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        DifficultyLevel otherData = (DifficultyLevel)obj;
        return name == otherData.name && buttonsCount == otherData.buttonsCount
             && pointsPerStep == otherData.pointsPerStep && TimeLimit == otherData.TimeLimit && repeatModeEnabled == otherData.repeatModeEnabled
             && gameSpeed == otherData.gameSpeed;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name, buttonsCount, pointsPerStep, TimeLimit, repeatModeEnabled, gameSpeed);
    }

}