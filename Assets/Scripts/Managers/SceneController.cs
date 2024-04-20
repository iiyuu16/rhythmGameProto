// SceneController.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void toRhythmMenu()
    {
        SceneManager.LoadScene("RhythmMenu");
    }

    public void toRGSong1()
    {
        SceneManager.LoadScene("RGSong1");
    }

    public void toRGSong2()
    {
        SceneManager.LoadScene("RGSong2");
    }

    public void toRGSong3()
    {
        SceneManager.LoadScene("RGSong3");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
