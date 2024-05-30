using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private string gameScene = "Game";

    public void Play ()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void Exit ()
    {
        Application.Quit();
    }

    void Start () {
    }
}
