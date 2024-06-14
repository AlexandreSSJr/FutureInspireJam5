using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedSceneLoader : MonoBehaviour
{
    public float delay = 10f;
    public string scene = "Game";

    private void LoadNextScene() {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void Start()
    {
        Invoke(nameof(LoadNextScene), delay);
    }

    private void Controls() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)) {
            LoadNextScene();
        }
    }

    void Update() {
        Controls();
    }
}
