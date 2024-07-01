using UnityEngine;
using UnityEngine.SceneManagement;

public class rhythmSceneController : MonoBehaviour
{
    public void toRhythmMenu()
    {
        LoadScene("RhythmMenu");
    }

    public void toRGSong1()
    {
        LoadScene("RGSong1");
    }

    public void toRGSong2()
    {
        LoadScene("RGSong2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadScene(string sceneName)
    {
        Scene currentScene = SceneManager.GetSceneByName(sceneName);
        if (currentScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }   

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        Time.timeScale = 1.0f;
    }
}
