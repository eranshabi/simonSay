using System;
using NUnit.Framework;
using Moq;

// Tests should mock as little as possible (integration approach), so changes in code won't cause tests to break
// In this example I mocked only the file system (in all but 2 tests), as it is the only external dependency of the tested code
public class ConfigLoaderTest
{
    [Test, Description("Should deserialize valid JSON file and return result")]
    public void _0_ShouldDeserializeValidJsonFileAndReturnResult()
    {
        ConfigLoader configLoader = new(new JsonConfigLoad(), (string path) => ConfigLoadTestFixture.validJsonData);
        GameConfig result = configLoader.LoadConfig();

        Assert.AreEqual(2, result.difficultyLevels.Length);
        Assert.AreEqual(new DifficultyLevel { name = "Easy", buttonsCount = 4, pointsPerStep = 1, TimeLimit = 50, repeatModeEnabled = true, gameSpeed = 1f }, result.difficultyLevels[0]);
        Assert.AreEqual(new DifficultyLevel { name = "Medium", buttonsCount = 5, pointsPerStep = 2, TimeLimit = 45, repeatModeEnabled = true, gameSpeed = 1.25f }, result.difficultyLevels[1]);
    }

    [Test, Description("Should deserialize valid XML file and return result")]
    public void _1_ShouldDeserializeValidXMLFileAndReturnResult()
    {
        ConfigLoader configLoader = new(new XmlConfigLoad(), (string path) => ConfigLoadTestFixture.validXmldata);
        GameConfig result = configLoader.LoadConfig();

        Assert.AreEqual(2, result.difficultyLevels.Length);
        Assert.AreEqual(new DifficultyLevel { name = "Easy", buttonsCount = 4, pointsPerStep = 1, TimeLimit = 50, repeatModeEnabled = true, gameSpeed = 1f }, result.difficultyLevels[0]);
        Assert.AreEqual(new DifficultyLevel { name = "Medium", buttonsCount = 5, pointsPerStep = 2, TimeLimit = 45, repeatModeEnabled = true, gameSpeed = 1.25f }, result.difficultyLevels[1]);
    }

    [Test, Description("Should throw error if number of buttons is less than 2")]
    public void _2_ShouldThrowErrorIfNumberOfButtonsIsLessThan2()
    {
        ConfigLoader configLoader = new(new JsonConfigLoad(), (string path) => ConfigLoadTestFixture.JsonWithTooFewButtons);
        Assert.Throws<Exception>(() => configLoader.LoadConfig(), ConfigLoader.INVALID_BUTTONS_COUNT_EXCEPTION);
    }

    [Test, Description("Should throw error if number of buttons is more than 6")]
    public void _3_ShouldThrowErrorIfNumberOfButtonsIsMoreThan6()
    {
        ConfigLoader configLoader = new(new JsonConfigLoad(), (string path) => ConfigLoadTestFixture.JsonWithTooManyButtons);
        Assert.Throws<Exception>(() => configLoader.LoadConfig(), ConfigLoader.INVALID_BUTTONS_COUNT_EXCEPTION);
    }

    [Test, Description("JSON loading sanity without file mock")] // It's important to have a test that runs without mocking the file system
    public void _4_JsonSanity()
    {
        var mock = new Mock<string>();
        ConfigLoader configLoader = new(new JsonConfigLoad());
        Assert.DoesNotThrow(() => configLoader.LoadConfig());
    }

    [Test, Description("XML loading sanity without file mock")] // It's important to have a test that runs without mocking the file system
    public void _5_XmlSanity()
    {
        ConfigLoader configLoader = new(new XmlConfigLoad());
        Assert.DoesNotThrow(() => configLoader.LoadConfig());
    }
}
