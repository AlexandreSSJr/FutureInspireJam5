using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private readonly string menuScene = "Menu";
    private readonly string gameScene = "Game";

    public void Play() {
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }

    public void Menu() {
        SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
    }

    public void Exit() {
        Application.Quit();
    }
}
