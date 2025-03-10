using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    MainManu,
    Loading,
    Level
}

public class LoadScene : MonoBehaviour
{
    public void LoadLoadingScene()
    {
        SceneManager.LoadScene("Loading");
    }
}
