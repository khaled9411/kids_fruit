 using UnityEngine;

[System.Serializable]
public class Character
{
    public string name;
    public GameObject prefab;
    public Sprite thumbnail;
}

public class CharacterManager : MonoBehaviour
{
    public Character[] availableCharacters;
    private const string SELECTED_CHARACTER_KEY = "SelectedCharacter";

    public static CharacterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(int index)
    {
        if (index >= 0 && index < availableCharacters.Length)
        {
            PlayerPrefs.SetInt(SELECTED_CHARACTER_KEY, index);
            PlayerPrefs.Save();
        }
    }

    public Character GetSelectedCharacter()
    {
        int index = PlayerPrefs.GetInt(SELECTED_CHARACTER_KEY, 0);
        return availableCharacters[index];
    }
}

