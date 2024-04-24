using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
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

    public void toRGSong3()
    {
        LoadScene("RGSong3");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadScene(string sceneName)
    {
        // Check if the scene is already loaded, then unload it
        Scene currentScene = SceneManager.GetSceneByName(sceneName);
        if (currentScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        // Load the scene with Single mode to ensure it's a new instance
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        Time.timeScale = 1.0f;
    }
}
