using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private GameplayButton gameplayButtonPrefab;
    [SerializeField] private GameplayButtonsContainer gameplayButtonsContainer;
    [SerializeField] private AudioClip soundClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private GameplayButton[] gameplayButtons;
    private Color[] buttonColors;

    private void Awake()
    {
        buttonColors = new Color[] {
        HexaToColor("#F06A8A"),
        HexaToColor("#8FBDDF"),
        HexaToColor("#FAE360"),
        HexaToColor("#90E500"),
        HexaToColor("#3CE3B9"),
        HexaToColor("#F48E35")};

        gameplayButtons = new GameplayButton[gameLogic.buttonsCount];
        RenderButtons();

        gameLogic.TimerChanged += UpdateTimerText; // Split between Awake And Start so time and score data will be available to the UI at Start
        gameLogic.ScoreChanged += UpdateScoreText;
        gameLogic.RepeatAll += RepeatAllStepsCoroutine;
        gameLogic.RepeatNew += RepeatLastStepCoroutine;
        gameLogic.ShowWinPanel += ShowWinPanel;
        gameLogic.ShowLosePanel += ShowLosePanel;
    }

    private void Start()
    {
        UpdateTimerText();
        UpdateScoreText();

        gameplayButtonsContainer.buttonsCount = gameLogic.buttonsCount;
        gameplayButtonsContainer.onPointerMoveCallback += OnGameplayPointerMove;
        gameplayButtonsContainer.onPointerClickCallback += OnGameplayPointerClick;
        gameplayButtonsContainer.onPointerDownCallback += OnGameplayPointerDown;
        gameplayButtonsContainer.onPointerUpCallback += OnGameplayPointerUp;
    }

    // Should have a generic solution instead of a long list
    private void OnDestroy()
    {
        gameLogic.TimerChanged -= UpdateTimerText;
        gameLogic.ScoreChanged -= UpdateScoreText;
        gameLogic.RepeatAll -= RepeatAllStepsCoroutine;
        gameLogic.RepeatNew -= RepeatAllStepsCoroutine;
        gameplayButtonsContainer.onPointerMoveCallback -= OnGameplayPointerMove;
        gameplayButtonsContainer.onPointerClickCallback -= OnGameplayPointerClick;
        gameplayButtonsContainer.onPointerDownCallback -= OnGameplayPointerDown;
        gameplayButtonsContainer.onPointerUpCallback -= OnGameplayPointerUp;
        gameLogic.ShowWinPanel -= ShowWinPanel;
        gameLogic.ShowLosePanel -= ShowLosePanel;
    }

    public void ReturnToMainMenuButtonPresses()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnGameplayPointerMove(int buttonIndex)
    {
        if (gameLogic.currentState == GameState.PlayerInput)
        {
            HighlightButton(buttonIndex);

            if (gameLogic.lastButtonIndex != buttonIndex)
            {
                ClearButtonHighlight(gameLogic.lastButtonIndex);
                gameLogic.lastButtonIndex = buttonIndex;
            }
        }
    }

    void OnGameplayPointerClick(int buttonIndex)
    {
        if (gameLogic.currentState == GameState.PlayerInput)
        {
            gameLogic.ValidatePlayerInput(buttonIndex);
        }
    }

    void OnGameplayPointerDown(int buttonIndex)
    {
        if (gameLogic.currentState == GameState.PlayerInput)
        {
            DownButton(buttonIndex);
        }
    }

    void OnGameplayPointerUp(int buttonIndex)
    {
        if (gameLogic.currentState == GameState.PlayerInput)
        {
            ClearButtonHighlight(buttonIndex);
        }
    }

    void RenderButtons()
    {
        for (int i = 0; i < gameLogic.buttonsCount; i++)
        {
            RenderGamplayButtonAt(i);
            ClearButtonHighlight(i);
        }
    }

    private void UpdateTimerText()
    {
        timerText.SetText(Mathf.Ceil(gameLogic.timeLimit).ToString());
    }

    private void UpdateScoreText()
    {
        scoreText.text = gameLogic.score.ToString();
    }

    void RenderGamplayButtonAt(int i)
    {
        GameplayButton gameplayButton = Instantiate(gameplayButtonPrefab, gameplayButtonsContainer.transform);
        gameplayButton.SetSizePositionAndColor(gameLogic.buttonsCount, i, buttonColors[i]);

        gameplayButtons[i] = gameplayButton;
    }


    private void ClearButtonHighlight(int index)
    {
        gameplayButtons[index].SetColor(ChangeAlphaTo(buttonColors[index], 0.5f));
        gameplayButtons[index].PointerExit();
    }

    private void HighlightButton(int index)
    {
        gameplayButtons[index].SetColor(ChangeAlphaTo(buttonColors[index], 1f));
        gameplayButtons[index].Hover();
    }

    private IEnumerator RepeatStep(int index)
    {
        ClearButtonHighlight(gameLogic.lastButtonIndex);
        HighlightButton(gameLogic.steps[index]);
        PlayButtonSound(gameLogic.steps[index]);
        yield return new WaitForSeconds(.7f);
        ClearButtonHighlight(gameLogic.steps[index]);
        yield return new WaitForSeconds(.3f);
    }

    private void DownButton(int index)
    {
        PlayButtonSound(index);
        gameplayButtons[index].SetColor(DarkenBy(buttonColors[index], .3f));
    }

    private Color ChangeAlphaTo(Color color, float alpha)
    {
        return new Color(
           color.r,
           color.g,
           color.b,
           alpha
       );
    }

    private Color DarkenBy(Color color, float dark)
    {
        float darkenFactor = 1 - dark;

        return new Color(
            color.r * darkenFactor,
            color.g * darkenFactor,
            color.b * darkenFactor,
            1f
        );
    }

    // I generate all buttons sound from one mp3 sound by changing the pitch
    private void PlayButtonSound(int buttonIndex)
    {
        if (gameLogic.currentState != GameState.EndGameScreen)
        {
            audioSource.clip = soundClip;
            audioSource.pitch = .85f + buttonIndex * .15f;
            audioSource.Play();
        }
    }

    // I prefer async await to coroutine/yield as it's more readable in my opinion,
    // but in this exercise desired unity version it's to complicated to cancel tasks after object was destroyed so I decided against it in those situations
    private void RepeatAllStepsCoroutine()
    {
        StartCoroutine(RepeatAllSteps());
    }

    private IEnumerator RepeatAllSteps()
    {
        for (int i = 0; i < gameLogic.steps.Count; i++)
        {
            yield return RepeatStep(i);
        }
        gameLogic.RepeatAnimationComplete();
    }

    private void RepeatLastStepCoroutine()
    {
        StartCoroutine(RepeatLastStep());
    }

    private IEnumerator RepeatLastStep()
    {
        yield return RepeatStep(gameLogic.steps.Count - 1);
        gameLogic.RepeatAnimationComplete();
    }

    private void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    private void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    private static Color HexaToColor(string hexValue)
    {
        ColorUtility.TryParseHtmlString(hexValue, out Color color);
        return color;
    }
}
