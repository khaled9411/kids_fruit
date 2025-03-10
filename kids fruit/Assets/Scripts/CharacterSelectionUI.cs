using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectionUI : MonoBehaviour
{
    [SerializeField] private Transform characterDisplayPoint;
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Vector2 buttonSize = new Vector2(100, 100);
    [SerializeField] private float spacing = 10f;
    [SerializeField] private int buttonsPerRow = 3;

    private GameObject currentDisplayedCharacter;
    private CharacterManager characterManager;

    private void Start()
    {
        characterManager = CharacterManager.Instance;
        CreateCharacterButtons();
        UpdateCharacterDisplay();
    }

    private void CreateCharacterButtons()
    {
        RectTransform containerRect = buttonsContainer as RectTransform;
        float containerWidth = containerRect.rect.width;

        float startX = -(containerWidth / 2) + (buttonSize.x / 2);
        float currentX = startX;
        float currentY = 0;
        int currentColumn = 0;

        for (int i = 0; i < characterManager.availableCharacters.Length; i++)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonsContainer);
            RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();

            rectTransform.sizeDelta = buttonSize;
            rectTransform.anchoredPosition = new Vector2(currentX, currentY);

            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = characterManager.availableCharacters[i].name;
            }


            Image buttonImage = buttonObj.GetComponent<Image>();
            if (buttonImage != null && characterManager.availableCharacters[i].thumbnail != null)
            {
                buttonImage.sprite = characterManager.availableCharacters[i].thumbnail;
            }

            int characterIndex = i;
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() => OnCharacterSelected(characterIndex));

            currentColumn++;
            if (currentColumn >= buttonsPerRow)
            {
                currentColumn = 0;
                currentX = startX;
                currentY -= (buttonSize.y + spacing);
            }
            else
            {
                currentX += buttonSize.x + spacing;
            }
        }
    }

    public void OnCharacterSelected(int index)
    {
        characterManager.SelectCharacter(index);
        UpdateCharacterDisplay();
    }

    private void UpdateCharacterDisplay()
    {
        if (currentDisplayedCharacter != null)
        {
            Destroy(currentDisplayedCharacter);
        }

        Character selectedCharacter = characterManager.GetSelectedCharacter();
        currentDisplayedCharacter = Instantiate(selectedCharacter.prefab, characterDisplayPoint);
    }
}