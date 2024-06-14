using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private readonly string menuScene = "Menu";
    // private readonly string gameScene = "Game";
    private readonly string introScene = "Intro";

    public void Play() {
        SceneManager.LoadScene(introScene, LoadSceneMode.Single);
    }

    public void Menu() {
        SceneManager.LoadScene(menuScene, LoadSceneMode.Single);
    }

    public void Exit() {
        Application.Quit();
    }
}
