using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

// All MainMenu code touches UI, so I didn't split to MVC/MVP.
public class MainMenu : MonoBehaviour
{
    [SerializeField] private DifficultyToggle difficultyTogglePrefab;
    [SerializeField] private TMP_InputField PlayerNameInput;
    [SerializeField] private Button StartGameButton;
    [SerializeField] private ToggleGroup difficultyToggleGroup;
    [SerializeField] private PlayerSettingsSO playerSettingsSO;

    void Awake()
    {
        playerSettingsSO.gameConfig = new ConfigLoader(new JsonConfigLoad()).LoadConfig();
        StartGameButton.interactable = false;

        RenderDifficultyToggles();
        SelectFirstDifficultyToggle();
    }

    void RenderDifficultyToggles()
    {
        string[] names = playerSettingsSO.GetLevelNames();

        for (int i = 0; i < names.Length; i++)
        {
            DifficultyToggle button = Instantiate(difficultyTogglePrefab, difficultyToggleGroup.transform);
            button.SetText(names[i]);
            button.SetToggleGroup(difficultyToggleGroup);
            int index = i;
            button.GetComponent<Toggle>().onValueChanged.AddListener(value => SelectDifficultyLevel(index, value));
        }
    }

    void SelectFirstDifficultyToggle()
    {
        difficultyToggleGroup.GetComponentsInChildren<DifficultyToggle>().First().Select();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    void SelectDifficultyLevel(int difficultyLevelIndex, bool isSelected)
    {
        if (isSelected)
        {
            playerSettingsSO.selectedDifficultyLevel = difficultyLevelIndex;
        }
    }

    public void OnPlayerNameInputValueChange()
    {
        if (PlayerNameInput.text.Trim().Length == 0)
        {
            StartGameButton.interactable = false;
        }
        else
        {
            playerSettingsSO.playerName = PlayerNameInput.text;
            StartGameButton.interactable = true;
        }
    }
}
