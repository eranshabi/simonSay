using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

// Using a state machine to simplify gameplay logic
public enum GameState
{
    PlayerInput,
    Repeat,
    EndGameScreen
}

// This class is purly for game business logic, free from all UI/View concerns which are in the GamePresenter class
public class GameLogic : MonoBehaviour
{
    [SerializeField] private PlayerSettingsSO playerSettingsSO;
    [SerializeField] private LeaderboardList leaderboardList;

    public event Action TimerChanged;
    public event Action ScoreChanged;
    public event Action RepeatAll;
    public event Action RepeatNew;
    public event Action ShowWinPanel;
    public event Action ShowLosePanel;

    public int buttonsCount { get; private set; }
    public int lastButtonIndex;
    public float timeLimit { get; private set; }
    public List<int> steps = new();
    public int playerInputCountAtCurrentTurn { get; private set; }
    public int score = 0;
    public GameState currentState = GameState.Repeat;

    private void Awake()
    {
        ReadPlayerSettings();
    }

    void Start()
    {
        RepeatStateStart();
    }

    private void Update()
    {
        if (currentState == GameState.PlayerInput || currentState == GameState.Repeat)
        {
            if (timeLimit <= 0f)
            {
                EndGame();
            }
            else
            {
                timeLimit -= Time.deltaTime;
                TimerChanged.Invoke();
            }
        }
    }

    private void ReadPlayerSettings()
    {
        buttonsCount = playerSettingsSO.GetButtonsCount();
        timeLimit = playerSettingsSO.GetTimeLimit();
    }

    async void RepeatStateStart()
    {
        currentState = GameState.Repeat;
        await Task.Delay(550); // This should move to the presenter, due to time constraints I was unable to fix this

        AddStep();
        if (playerSettingsSO.IsRepeatEnabled())
        {
            RepeatAll.Invoke();
        }
        else
        {
            RepeatNew.Invoke();
        }

    }

    public void RepeatAnimationComplete()
    {
        PlayerInputStateStart();
    }

    void PlayerInputStateStart()
    {
        currentState = GameState.PlayerInput;
        playerInputCountAtCurrentTurn = 0;
    }

    void PlayerInputStateEnd()
    {
        RepeatStateStart();
    }

    private bool HasWon(bool pressedWrongAnswer)
    {
        if (pressedWrongAnswer)
        {
            return false;
        }
        else
        {
            return (timeLimit <= 0 && score > 0);
        }
    }

    private void EndGame(bool pressedWrongAnswer = false)
    {
        currentState = GameState.EndGameScreen;
        bool hasWon = HasWon(pressedWrongAnswer);

        if (hasWon)
        {
            AddNewScoreToLeaderboard();
            ShowWinPanel.Invoke();
        }
        else
        {
            ShowLosePanel.Invoke();
        }
    }

    private void AddNewScoreToLeaderboard()
    {
        LeaderboardService leaderboardService = new(Application.persistentDataPath);
        leaderboardService.AddScoreData(new ScoreData { name = playerSettingsSO.playerName, score = score });
    }

    void AddStep()
    {
        steps.Add(GenerateRandomNewStep());
    }

    private int GenerateRandomNewStep()
    {
        return UnityEngine.Random.Range(0, buttonsCount); // I explicitly use UnityEngine.Random so I won't get System.Random as a side effect from imports changes
    }

    public void ValidatePlayerInput(int buttonIndex)
    {
        if (steps[playerInputCountAtCurrentTurn] == buttonIndex)
        {
            // Last player input was correct
            playerInputCountAtCurrentTurn++;
            if (playerInputCountAtCurrentTurn == steps.Count)
            {
                // All player inputs are correct
                score += playerSettingsSO.GetPointsPerStep();
                ScoreChanged.Invoke();
                PlayerInputStateEnd();
            }
        }
        else
        {
            // Incorrect
            EndGame(true);
        }

    }
}
