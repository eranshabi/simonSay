public static class ConfigLoadTestFixture
{
    public static string JsonWithTooFewButtons = @"
    {""difficultyLevels"": [
        {
            ""name"": ""Easy"",
            ""buttonsCount"": 1,
            ""pointsPerStep"": 1,
            ""TimeLimit"": 50,
            ""repeatModeEnabled"": true,
            ""gameSpeed"": 1
        },
        {
            ""name"": ""Medium"",
            ""buttonsCount"": 5,
            ""pointsPerStep"": 2,
            ""TimeLimit"": 45,
            ""repeatModeEnabled"": true,
            ""gameSpeed"": 1.25
        }
    ]
}
";
    public static string JsonWithTooManyButtons = @"
    {""difficultyLevels"": [
        {
            ""name"": ""Easy"",
            ""buttonsCount"": 2,
            ""pointsPerStep"": 1,
            ""TimeLimit"": 50,
            ""repeatModeEnabled"": true,
            ""gameSpeed"": 1
        },
        {
            ""name"": ""Medium"",
            ""buttonsCount"": 7,
            ""pointsPerStep"": 2,
            ""TimeLimit"": 45,
            ""repeatModeEnabled"": true,
            ""gameSpeed"": 1.25
        }
    ]
}
";
    public static string validJsonData = @"
    {""difficultyLevels"": [
        {
            ""name"": ""Easy"",
            ""buttonsCount"": 4,
            ""pointsPerStep"": 1,
            ""TimeLimit"": 50,
            ""repeatModeEnabled"": true,
            ""gameSpeed"": 1
        },
        {
            ""name"": ""Medium"",
            ""buttonsCount"": 5,
            ""pointsPerStep"": 2,
            ""TimeLimit"": 45,
            ""repeatModeEnabled"": true,
            ""gameSpeed"": 1.25
        }
    ]
}
";
    public static string validXmldata = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<gameConfig>
	<difficultyLevels>
		<difficultyLevel name=""Easy"" buttonsCount=""4"" pointsPerStep=""1"" TimeLimit=""50"" repeatModeEnabled=""true"" gameSpeed=""1"">
		</difficultyLevel>
		<difficultyLevel name=""Medium"" buttonsCount=""5"" pointsPerStep=""2"" TimeLimit=""45"" repeatModeEnabled=""true"" gameSpeed=""1.25"">
		</difficultyLevel>
	</difficultyLevels>
</gameConfig>
";
}
