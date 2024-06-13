using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Env : MonoBehaviour
{
    public float timer;
    public bool countTime = true;
    [SerializeField] private TMP_Text timerText;
    private readonly string victoryScene = "Victory";

    private void UpdateTimer() {
        timer -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer % 60);
        string minutesText;

        if (minutes < 10) {
            minutesText = "0" + minutes.ToString();
        } else {
            minutesText = minutes.ToString();
        }

        if (timerText) {
            timerText.text = "0" + Mathf.FloorToInt(timer / 60).ToString() + ":" + minutesText;
        }

        if (timer < 1) {
            Victory();
        }
    }

    private void Victory() {
      countTime = false;
      SceneManager.LoadScene(victoryScene, LoadSceneMode.Single);
    }

    private void Awake()
    {
        timer = 300;
        countTime = true;
    }

    void Update () {
        if (countTime) {
            UpdateTimer();
        }
    }
}