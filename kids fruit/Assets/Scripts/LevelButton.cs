using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LevelData
{
    //public string levelName;
    //public Sprite levelIcon;
    public string levelPrefabName;
    //public string levelDescription;
}

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button button;
    //[SerializeField] private TMP_Text levelNameText;
    //[SerializeField] private Image levelIconImage;
    //[SerializeField] private Image buttonBackground;
    //[SerializeField] private GameObject lockIcon;

    //[Header("Status Colors")]
    //[SerializeField] private Color normalColor = Color.white;
    //[SerializeField] private Color selectedColor = Color.yellow;
    //[SerializeField] private Color lockedColor = Color.gray;

    private int levelIndex;
    private bool isUnlocked;

    public void SetupButton(int index, LevelData data, bool unlocked)
    {
        levelIndex = index;
        //levelNameText.text = data.levelName;
        //levelIconImage.sprite = data.levelIcon;
        isUnlocked = unlocked;

        //lockIcon.SetActive(!isUnlocked);
        button.interactable = isUnlocked;

        //buttonBackground.color = isUnlocked ? normalColor : lockedColor;
    }

    public void SetSelected(bool selected)
    {
        //if (isUnlocked)
        //{
        //    buttonBackground.color = selected ? selectedColor : normalColor;
        //}
    }

    public Button GetButton()
    {
        return button;
    }
}