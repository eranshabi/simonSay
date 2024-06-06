using System;
using NUnit.Framework;
using Moq;

public class JsonLoaderTestsWithMocks
{
	private readonly ReadFileFunc mockReadFileFunc = new ReadFileFunc(path =>
		path == "Assets/Configs/config.json" ? ConfigLoadTestFixture.validJsonData : null);

	[Test, Description("Should deserialize valid JSON file and return result")]
	public void _0_ShouldDeserializeValidJsonFileAndReturnResult()
	{
		var jsonLoader = new JsonConfigLoad();

		var result = jsonLoader.LoadConfig(mockReadFileFunc);
		Assert.AreEqual(5, result.difficultyLevels[1].buttonsCount);
	}
}