using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private List<LevelData> levels = new List<LevelData>();
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelButtonsContainer;

    [Header("UI Settings")]
    [SerializeField] private Button playButton;
    [SerializeField] private string gameplaySceneName = "LevelScene";

    private int selectedLevelIndex = -1;
    private int highestUnlockedLevel = 0;

    private void Start()
    {
        LoadPlayerProgress();
        CreateLevelButtons();
        SelectLatestUnlockedLevel();

        playButton.onClick.AddListener(LoadSelectedLevel);
    }

    private void LoadPlayerProgress()
    {
        highestUnlockedLevel = PlayerPrefs.GetInt("HighestUnlockedLevel", 0);
    }

    private void CreateLevelButtons()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            //GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonsContainer);
            GameObject buttonObj = levelButtonsContainer.GetChild(i).gameObject;
            LevelButton levelButton = buttonObj.GetComponent<LevelButton>();

            levelButton.SetupButton(i, levels[i], i <= highestUnlockedLevel);

            int index = i;
            levelButton.GetButton().onClick.AddListener(() => SelectLevel(index));
        }
    }

    private void SelectLatestUnlockedLevel()
    {
        if (highestUnlockedLevel >= 0 && highestUnlockedLevel < levels.Count)
        {
            SelectLevel(highestUnlockedLevel);
        }
    }

    private void SelectLevel(int index)
    {
        if (selectedLevelIndex >= 0)
        {
            LevelButton prevButton = levelButtonsContainer.GetChild(selectedLevelIndex).GetComponent<LevelButton>();
            prevButton.SetSelected(false);
        }

        selectedLevelIndex = index;
        LevelButton newButton = levelButtonsContainer.GetChild(selectedLevelIndex).GetComponent<LevelButton>();
        newButton.SetSelected(true);

        playButton.interactable = true;
    }

    private void LoadSelectedLevel()
    {
        if (selectedLevelIndex >= 0)
        {
            PlayerPrefs.SetString("SelectedLevelPrefab", levels[selectedLevelIndex].levelPrefabName);
            PlayerPrefs.Save();

            SceneManager.LoadScene(gameplaySceneName);
        }
    }

    public void UnlockNextLevel(int completedLevelIndex)
    {
        int nextLevelIndex = completedLevelIndex + 1;

        if (nextLevelIndex > highestUnlockedLevel && nextLevelIndex < levels.Count)
        {
            highestUnlockedLevel = nextLevelIndex;
            PlayerPrefs.SetInt("HighestUnlockedLevel", highestUnlockedLevel);
            PlayerPrefs.Save();
        }
    }
}