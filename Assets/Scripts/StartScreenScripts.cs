using UnityEngine;
using UnityEngine.SceneManagement;


public class StartScreenScripts : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
