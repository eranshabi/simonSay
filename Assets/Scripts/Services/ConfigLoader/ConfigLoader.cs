using System;
using System.IO;
using System.Linq;

public class ConfigLoader
{
    private IConfigLoadStrategy ConfigLoadStrategy { get; set; }
    private readonly ReadFileFunc readFileFunc;
    private GameConfig loadedConfig;
    public const string INVALID_BUTTONS_COUNT_EXCEPTION = "Error: All config file buttons count must be between 2 and 6";

    public ConfigLoader(IConfigLoadStrategy configLoadStrategy, ReadFileFunc readFileFunc = null)
    {
        this.readFileFunc = readFileFunc ?? File.ReadAllText;
        this.ConfigLoadStrategy = configLoadStrategy;//new JsonConfigLoad();
    }

    public GameConfig LoadConfig()
    {
        loadedConfig = ConfigLoadStrategy.LoadConfig(readFileFunc);
        ValidateConfig();
        return loadedConfig;
    }

    private void ValidateConfig()
    {
        if (loadedConfig.difficultyLevels.Select(level => level.buttonsCount).Any(IsInvalidateButtonsCount))
        {
            throw new Exception(INVALID_BUTTONS_COUNT_EXCEPTION);
        }
    }

    private static bool IsInvalidateButtonsCount(int buttonsCount)
    {
        return buttonsCount < 2 || buttonsCount > 6;
    }
}


public interface IConfigLoadStrategy
{
    GameConfig LoadConfig(ReadFileFunc readFileFunc);
}

public delegate string ReadFileFunc(string path);
