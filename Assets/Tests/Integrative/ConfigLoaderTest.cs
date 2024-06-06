using System;
using NUnit.Framework;

// Tests should mock as little as possible (integration approach), so changes in code won't cause tests to break
// In this example I mocked only the file system (in all but 2 tests), as it is the only external dependency of the tested code
public class ConfigLoaderTest
{
	private readonly Func<string, ReadFileFunc> mockReadFileFunc = result => (path =>
		path == "Assets/Configs/config.json" ? result : null);

	[Test, Description("Should deserialize valid JSON file and return result")]
	public void _0_ShouldDeserializeValidJsonFileAndReturnResult()
	{
		ConfigLoader configLoader = new(new JsonConfigLoad(), mockReadFileFunc(ConfigLoadTestFixture.validJsonData));
		GameConfig result = configLoader.LoadConfig();

		Assert.AreEqual(2, result.difficultyLevels.Length);
		Assert.AreEqual(
			new DifficultyLevel
			{
				name = "Easy", buttonsCount = 4, pointsPerStep = 1, TimeLimit = 50, repeatModeEnabled = true,
				gameSpeed = 1f
			}, result.difficultyLevels[0]);
		Assert.AreEqual(
			new DifficultyLevel
			{
				name = "Medium", buttonsCount = 5, pointsPerStep = 2, TimeLimit = 45, repeatModeEnabled = true,
				gameSpeed = 1.25f
			}, result.difficultyLevels[1]);
	}

	[Test, Description("Should throw error if number of buttons is less than 2")]
	public void _1_ShouldThrowErrorIfNumberOfButtonsIsLessThan2()
	{
		ConfigLoader configLoader =
			new(new JsonConfigLoad(), mockReadFileFunc(ConfigLoadTestFixture.JsonWithTooFewButtons));
		Assert.Throws<Exception>(() => configLoader.LoadConfig(), ConfigLoader.INVALID_BUTTONS_COUNT_EXCEPTION);
	}

	[Test, Description("Should throw error if number of buttons is more than 6")]
	public void _2_ShouldThrowErrorIfNumberOfButtonsIsMoreThan6()
	{
		ConfigLoader configLoader =
			new(new JsonConfigLoad(), mockReadFileFunc(ConfigLoadTestFixture.JsonWithTooManyButtons));
		Assert.Throws<Exception>(() => configLoader.LoadConfig(), ConfigLoader.INVALID_BUTTONS_COUNT_EXCEPTION);
	}
}