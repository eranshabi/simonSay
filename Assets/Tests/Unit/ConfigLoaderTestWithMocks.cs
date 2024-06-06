using System;
using NUnit.Framework;
using Moq;

public class ConfigLoaderTestWithMocks
{
	[Test, Description("Should deserialize valid JSON file and return result")]
	public void _0_ShouldDeserializeValidJsonFileAndReturnResult()
	{
		var difficultyLevel1 = new DifficultyLevel
		{
			name = "Easy", buttonsCount = 4, pointsPerStep = 1, TimeLimit = 50, repeatModeEnabled = true, gameSpeed = 1f
		};
		
		var difficultyLevel2 = new DifficultyLevel
		{
			name = "Medium", buttonsCount = 5, pointsPerStep = 2, TimeLimit = 45, repeatModeEnabled = true,
			gameSpeed = 1.25f
		};

		var gameConfig = new GameConfig {difficultyLevels = new[] {difficultyLevel1, difficultyLevel2}};
		var jsonLoaderMock = new Mock<IConfigLoadStrategy>();

		var readFileFunc = new ReadFileFunc((string path) => null);
		jsonLoaderMock.Setup(foo => foo.LoadConfig(readFileFunc))
			.Returns(gameConfig);

		ConfigLoader configLoader = new(jsonLoaderMock.Object, readFileFunc);
		GameConfig result = configLoader.LoadConfig();

		Assert.AreEqual(2, result.difficultyLevels.Length);
		Assert.AreEqual(difficultyLevel1, result.difficultyLevels[0]);
		Assert.AreEqual(difficultyLevel2, result.difficultyLevels[1]);
	}

	[Test, Description("Should throw error if number of buttons is less than 2")]
	public void _2_ShouldThrowErrorIfNumberOfButtonsIsLessThan2()
	{
		var difficultyLevelWithOneButton = new DifficultyLevel
		{
			name = "Easy", buttonsCount = 1, pointsPerStep = 1, TimeLimit = 50, repeatModeEnabled = true, gameSpeed = 1f
		};

		var gameConfig = new GameConfig {difficultyLevels = new[] {difficultyLevelWithOneButton}};
		var jsonLoaderMock = new Mock<IConfigLoadStrategy>();
		var readFileFunc = new ReadFileFunc((string path) => null);
		jsonLoaderMock.Setup(foo => foo.LoadConfig(readFileFunc))
			.Returns(gameConfig);


		ConfigLoader configLoader =
			new(jsonLoaderMock.Object, readFileFunc);
		Assert.Throws<Exception>(() => configLoader.LoadConfig(), ConfigLoader.INVALID_BUTTONS_COUNT_EXCEPTION);
	}

	[Test, Description("Should throw error if number of buttons is more than 6")]
	public void _3_ShouldThrowErrorIfNumberOfButtonsIsMoreThan6()
	{
		var difficultyLevelWithOneButton = new DifficultyLevel
		{
			name = "Easy", buttonsCount = 7, pointsPerStep = 1, TimeLimit = 50, repeatModeEnabled = true, gameSpeed = 1f
		};

		var gameConfig = new GameConfig {difficultyLevels = new[] {difficultyLevelWithOneButton}};
		var jsonLoaderMock = new Mock<IConfigLoadStrategy>();
		var readFileFunc = new ReadFileFunc((string path) => null);
		jsonLoaderMock.Setup(foo => foo.LoadConfig(readFileFunc))
			.Returns(gameConfig);

		ConfigLoader configLoader =
			new(jsonLoaderMock.Object, readFileFunc);
		Assert.Throws<Exception>(() => configLoader.LoadConfig(), ConfigLoader.INVALID_BUTTONS_COUNT_EXCEPTION);
	}
}