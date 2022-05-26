using UnityEngine;
using UnityEngine.SceneManagement;


public class BreakoutSceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void LoadStartScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }
    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void LoadWinScene()
    {
        SceneManager.LoadScene("Win");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
