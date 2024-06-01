using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private readonly string gameScene = "Game";

    public void Play() {
        SceneManager.LoadScene(gameScene);
    }

    public void Exit() {
        Application.Quit();
    }
}
